using Americasa.Demo.CustomActivities.AuthHandler;
using Americasa.Demo.CustomActivities.Bookmark;
using Americasa.Demo.CustomActivities.Models;
using Elsa;
using Elsa.Activities.Http.Contracts;
using Elsa.Activities.Http.Extensions;
using Elsa.Activities.Http.Options;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Open.Linq.AsyncExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Americasa.Demo.CustomActivities.Middleware
{
    public class CustomActivityMiddleware
    {

        private readonly ICustomAuthorizationHandler _customAuthorizationHandler1;
        public CustomActivityMiddleware(ICustomAuthorizationHandler customAuthorizationHandler)
        {
            _customAuthorizationHandler1 = customAuthorizationHandler;
        }
        private readonly RequestDelegate _next;
        public CustomActivityMiddleware(RequestDelegate next) => _next = next;
        public async Task InvokeAsync(
            HttpContext httpContext,
            IWorkflowLaunchpad workflowLaunchpad,
            IWorkflowInstanceStore workflowInstanceStore,
            IWorkflowRegistry workflowRegistry,
            IWorkflowBlueprintReflector workflowBlueprintReflector,
            IRouteMatcher routeMatcher,
            ITenantAccessor tenantAccessor,
            IEnumerable<IHttpRequestBodyParser> contentParsers)
        {
            var basePath = httpContext.Request.Path.Value;
            var path = httpContext.Request.Path.Value;

            if (path == null)
            {
                await _next(httpContext);
                return;
            }

            var request = httpContext.Request;
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
            const string activityType = nameof(CustomSignal);
            var bookmark = new SignalCustomBookmark();
            var collectWorkflowsContext = new WorkflowsQuery(activityType, bookmark, correlationId, default, default, tenantId);
            var pendingWorkflows = await workflowLaunchpad.FindWorkflowsAsync(collectWorkflowsContext, cancellationToken).ToList();

            if (await HandleNoWorkflowsFoundAsync(httpContext, pendingWorkflows, basePath))
                return;

            if (await HandleMultipleWorkflowsFoundAsync(httpContext, pendingWorkflows, cancellationToken))
                return;

            var pendingWorkflow = pendingWorkflows.Single();
            var pendingWorkflowInstance = pendingWorkflow.WorkflowInstance ?? await workflowInstanceStore.FindByIdAsync(pendingWorkflow.WorkflowInstanceId, cancellationToken);

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
            var activityWrapper = workflowBlueprintWrapper.GetUnfilteredActivity<CustomSignal>(pendingWorkflow.ActivityId!)!;
            if (!await AuthorizeAsync(httpContext, activityWrapper, workflowBlueprint, pendingWorkflow, cancellationToken))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

        }
            private async Task<bool> AuthorizeAsync(
            HttpContext httpContext,
            IActivityBlueprintWrapper<CustomSignal> customActivity,
            IWorkflowBlueprint workflowBlueprint,
            CollectedWorkflow pendingWorkflow,
            CancellationToken cancellationToken)
        {
            var authorize = await customActivity.EvaluatePropertyValueAsync(x => x.Authorize, cancellationToken);

            if (!authorize)
                return true;

            var authorizationHandler = _customAuthorizationHandler1;

            return await authorizationHandler.AuthorizeAsync(new AuthorizeCustomContext(httpContext, customActivity, workflowBlueprint, pendingWorkflow.WorkflowInstanceId, cancellationToken));
        }



        private async Task<bool> HandleNoWorkflowsFoundAsync(HttpContext httpContext, IList<CollectedWorkflow> pendingWorkflows, PathString? basePath)
        {
            if (pendingWorkflows.Any())
                return false;

            // If a base path was configured, we are sure the requester tried to execute a workflow that doesn't exist.
            // Therefore, sending a 404 response seems appropriate instead of continuing with any subsequent middlewares.
            if (basePath != null)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return true;
            }

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

        private string? GetPath(PathString? basePath, HttpContext httpContext) => basePath != null
            ? httpContext.Request.Path.StartsWithSegments(basePath.Value, out _, out var remainingPath) ? remainingPath.Value : null
            : httpContext.Request.Path.Value;

    }
}
