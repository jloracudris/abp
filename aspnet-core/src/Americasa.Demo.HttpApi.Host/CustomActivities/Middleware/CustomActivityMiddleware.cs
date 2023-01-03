using Americasa.Demo.CustomActivities.AuthHandler;
using Americasa.Demo.CustomActivities.Bookmark;
using Americasa.Demo.CustomActivities.Models;
using Americasa.Demo.CustomActivities.Query;
using Elsa;
using Elsa.Activities.Http;
using Elsa.Activities.Http.Bookmarks;
using Elsa.Activities.Http.Contracts;
using Elsa.Activities.Http.Extensions;
using Elsa.Activities.Http.Models;
using Elsa.Activities.Http.Options;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Open.Linq.AsyncExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Americasa.Demo.CustomActivities.Middleware
{
    public class CustomActivityMiddleware
    {
        private readonly string pathCustomSignal = "/custom-signals/";
        private readonly ICustomAuthorizationHandler _customAuthorizationHandler;
        private readonly IBookmarkFinder _bookmarkFinder;
        private readonly IAuthorizationService _authorizationService;
        private string NameSignal;
        public CustomActivityMiddleware()
        {
        }
        private readonly RequestDelegate _next;
        public CustomActivityMiddleware(RequestDelegate next) => _next = next;
        public async Task InvokeAsync(
            HttpContext httpContext,
            IOptions<HttpActivityOptions> options,
            ITestWorkflowLaunchpad workflowLaunchpad,
            IWorkflowInstanceStore workflowInstanceStore,
            IWorkflowRegistry workflowRegistry,
            IWorkflowBlueprintReflector workflowBlueprintReflector,
            IRouteMatcher routeMatcher,
            ITenantAccessor tenantAccessor,
            IEnumerable<IHttpRequestBodyParser> contentParsers,
            ICustomAuthorizationHandler customAuthorizationHandler
            )
        {
            var basePath = httpContext.Request.Path.Value;
            var path = httpContext.Request.Path.Value;

            if (path == null || !path.StartsWith(pathCustomSignal))
            {
                await _next(httpContext);
                return;
            }

            var request = httpContext.Request;
            NameSignal = path.Split("/")[2];
            //var instanceId = await GetWorkFlowId(request);

            //if (string.IsNullOrEmpty(instanceId)) {
            //    await _next(httpContext);
            //    return;
            //}

            var cancellationToken = CancellationToken.None; // Prevent half-way request abortion (which also happens when WriteHttpResponse writes to the response).
            var method = httpContext.Request.Method!.ToLowerInvariant();

            var tenantId = await tenantAccessor.GetTenantIdAsync(cancellationToken);

            request.TryGetCorrelationId(out var correlationId);

            // Try to match inbound path.
            var routeTable = httpContext.RequestServices.GetRequiredService<IRouteTable>();

            var matchingRouteQuery =
                from route in routeTable
                let routeValues = routeMatcher.Match(route, path)
                where routeValues != null
                select new { route, routeValues };

            var matchingRoute = matchingRouteQuery.FirstOrDefault();
            var routeTemplate = matchingRoute?.route ?? path;
            var routeData = httpContext.GetRouteData();

            if (matchingRoute != null)
            {
                foreach (var routeValue in matchingRoute.routeValues!)
                    routeData.Values[routeValue.Key] = routeValue.Value;
            }

            // Create a workflow query using the selected route and HTTP method of the request.
            //const string activityType = nameof(CustomSignal);
            const string activityType = nameof(HttpEndpoint);
            //var bookmark = new SignalCustomBookmark();
            var bookmark = new HttpEndpointBookmark(routeTemplate, method);
            var collectWorkflowsContext = new WorkflowsQuery(activityType, bookmark, correlationId, default, default, tenantId);
            //var pendingWorkflows = await workflowLaunchpad.FindWorkflowsAsync(collectWorkflowsContext, cancellationToken).ToList();
            ////var pendingWorkflows = await CollectResumableAndStartableWorkflowsAsync(collectWorkflowsContext, cancellationToken, workflowLaunchpad).ToList();
            //if (await HandleNoWorkflowsFoundAsync(httpContext, pendingWorkflows, basePath))
            //    return;

            //if (await HandleMultipleWorkflowsFoundAsync(httpContext, pendingWorkflows, cancellationToken))
            //    return;

            //var pendingWorkflow = pendingWorkflows.Single();
            //var workflowInstanceId = pendingWorkflow.WorkflowInstanceId;
            //var pendingWorkflowInstance = pendingWorkflow.WorkflowInstance ?? await workflowInstanceStore.FindByIdAsync(pendingWorkflow.WorkflowInstanceId, cancellationToken);
            var workflowInstanceId = await GetInstanceid(request);
            if(workflowInstanceId == "")
            {
                await _next(httpContext);
                return;
            }
            var pendingWorkflowInstance = await workflowInstanceStore.FindByIdAsync(workflowInstanceId, cancellationToken);

            if (pendingWorkflowInstance is null)
            {
                await _next(httpContext);
                return;
            }

            var isTest = pendingWorkflowInstance.GetMetadata("isTest");

            var workflowBlueprint = (isTest != null && Convert.ToBoolean(isTest))
                ? await workflowRegistry.FindAsync(pendingWorkflowInstance.DefinitionId, VersionOptions.Latest, tenantId, cancellationToken)
                : await workflowRegistry.FindAsync(pendingWorkflowInstance.DefinitionId, VersionOptions.Published, tenantId, cancellationToken);

            var workflowBlueprintWrapper = await workflowBlueprintReflector.ReflectAsync(httpContext.RequestServices, workflowBlueprint, cancellationToken);
            //var activityWrapper = workflowBlueprintWrapper.GetUnfilteredActivity<CustomSignal>(pendingWorkflowInstance.LastExecutedActivityId!)!;

            foreach (var activity in pendingWorkflowInstance.BlockingActivities)
            {
                var activityWrapper = workflowBlueprintWrapper.GetUnfilteredActivity<CustomSignal>(activity.ActivityId!)!;
                if(activityWrapper != null)
                {
                    var signal = await activityWrapper.EvaluatePropertyValueAsync(x => x.Signal, cancellationToken);
                    if (signal == NameSignal)
                    {
                        if (!await AuthorizeAsync(httpContext, customAuthorizationHandler, activityWrapper, workflowBlueprint, workflowInstanceId, cancellationToken))
                        {
                            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return;
                        }
                        else
                        {
                            await _next(httpContext);
                            return;
                        }
                    }
                }
                
            }

            await _next(httpContext);
            return;



        }
            private async Task<bool> AuthorizeAsync(
            HttpContext httpContext,
            ICustomAuthorizationHandler customAuthorizationHandler,
            IActivityBlueprintWrapper<CustomSignal> customActivity,
            IWorkflowBlueprint workflowBlueprint,
            string workflowInstanceId,
            CancellationToken cancellationToken)
        {
            var authorize = await customActivity.EvaluatePropertyValueAsync(x => x.Authorize, cancellationToken);

            if (!authorize)
                return true;

            var context = new AuthorizeCustomContext(httpContext, customActivity, workflowBlueprint, workflowInstanceId, cancellationToken);

            return await customAuthorizationHandler.AuthorizeAsync(context);


            //var authorize = await customActivity.EvaluatePropertyValueAsync(x => x.Authorize, cancellationToken);

            //if (!authorize)
            //    return true;

            //var authorizationHandler = options.HttpEndpointAuthorizationHandlerFactory(httpContext.RequestServices);

            //return await authorizationHandler.AuthorizeAsync(new AuthorizeHttpEndpointContext(httpContext, customActivity, workflowBlueprint, workflowInstanceId, cancellationToken));
        }



        private async Task<bool> HandleNoWorkflowsFoundAsync(HttpContext httpContext, IList<CollectedWorkflow> pendingWorkflows, PathString? basePath)
        {
            if (pendingWorkflows.Any())
                return false;

            // If a base path was configured, we are sure the requester tried to execute a workflow that doesn't exist.
            // Therefore, sending a 404 response seems appropriate instead of continuing with any subsequent middlewares.
            //if (basePath != null)
            //{
            //    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            //    return true;
            //}

            // If no base path was configured on the other hand, the request could be targeting anything else and should be handled by subsequent middlewares. 
            await _next(httpContext);

            return true;
        }

        private async Task<bool> HandleMultipleWorkflowsFoundAsync(HttpContext httpContext, IList<CollectedWorkflow> pendingWorkflows, CancellationToken cancellationToken)
        {
            if (pendingWorkflows.Count <= 1)
                return false;

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var responseContent = JsonConvert.SerializeObject(new
            {
                errorMessage = "The call is ambiguous and matches multiple workflows.",
                workflows = pendingWorkflows
            });

            await httpContext.Response.WriteAsync(responseContent, cancellationToken);
            return true;
        }

        public async Task<string> GetWorkFlowId(HttpRequest request)
        {
            try
            {
                request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                //get body string here...
                var requestContent = Encoding.UTF8.GetString(buffer);
                var jRequest = JObject.Parse(requestContent);

                var workflowInstanceId = jRequest["workflowInstanceId"].ToString();
                return workflowInstanceId;
            }catch(Exception ex)
            {
                return "";
            }
            
        }


        public async Task<IEnumerable<CollectedWorkflow>> CollectResumableAndStartableWorkflowsAsync(WorkflowsQuery query, CancellationToken cancellationToken, IWorkflowLaunchpad workflowLaunchpad)
        {
            var bookmarkResultsQuery = query.Bookmark != null ? await _bookmarkFinder.FindBookmarksAsync(query.ActivityType, query.Bookmark, query.CorrelationId, query.TenantId, cancellationToken: cancellationToken) : default;
            var bookmarkResults = bookmarkResultsQuery?.ToList() ?? new List<BookmarkFinderResult>();
            var triggeredPendingWorkflows = bookmarkResults.Select(x => new CollectedWorkflow(x.WorkflowInstanceId, null, x.ActivityId)).ToList();
            var startableWorkflows = await workflowLaunchpad.FindStartableWorkflowsAsync(query, cancellationToken);
            var pendingWorkflows = triggeredPendingWorkflows.Concat(startableWorkflows.Select(x => new CollectedWorkflow(x.WorkflowInstance.Id, x.WorkflowInstance, x.ActivityId))).Distinct().ToList();

            return pendingWorkflows;
        }



        private string? GetPath(PathString? basePath, HttpContext httpContext) => basePath != null
            ? httpContext.Request.Path.StartsWithSegments(basePath.Value, out _, out var remainingPath) ? remainingPath.Value : null
            : httpContext.Request.Path.Value;


        private async Task<string> GetInstanceid(HttpRequest request)
        {
            var requestContent = await GetRequestBodyAsync(request);
            if (requestContent == "")
                return "";
            var json = JObject.Parse(requestContent);
            var instanceId = json["workflowInstanceId"];
            return instanceId == null ? "" : instanceId.ToString();

        }

        public async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            
            HttpRequestRewindExtensions.EnableBuffering(request);
            string strRequestBody = "";
            using (StreamReader reader = new StreamReader(
                request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true))
            {
                strRequestBody = await reader.ReadToEndAsync();
                
                request.Body.Position = 0;
            }

            return strRequestBody;
        }


        //private static string GetActionId(AuthorizationFilterContext context)
        //{
        //    var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
        //    var area = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue;
        //    var controller = controllerActionDescriptor.ControllerName;
        //    var action = controllerActionDescriptor.ActionName;

        //    return $"{area}:{controller}:{action}";
        //}

        //public async ValueTask<bool> AuthorizeAsync(HttpContext HttpContext, IActivityBlueprintWrapper<CustomSignal> CustomActivity, IWorkflowBlueprint WorkflowBlueprint, string WorkflowInstanceId, CancellationToken CancellationToken)
        //{
        //    var httpContext = HttpContext;
        //    var user = httpContext.User;
        //    var identity = user.Identity;

        //    if (identity == null)
        //        return false;

        //    if (identity.IsAuthenticated == false)
        //        return false;

        //    var cancellationToken = CancellationToken;
        //    var customActivity = CustomActivity;
        //    var policyName = await customActivity.EvaluatePropertyValueAsync(x => x.Policy, cancellationToken);

        //    if (string.IsNullOrWhiteSpace(policyName))
        //        return identity.IsAuthenticated;

        //    var resource = new HttpWorkflowResource(WorkflowBlueprint, customActivity.ActivityBlueprint, WorkflowInstanceId);
        //    var authorizationResult = await _authorizationService.AuthorizeAsync(user, resource, policyName);
        //    return authorizationResult.Succeeded;
        //}


    }
}
