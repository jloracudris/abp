using Americasa.Demo.CustomActivities.Models;
using Elsa.Activities.Http.Contracts;
using Elsa.Activities.Http.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Americasa.Demo.CustomActivities.AuthHandler
{
    public class AuthenticationBasedCustomAuthorizationHandler : ICustomAuthorizationHandler
    {

        private readonly IAuthorizationService _authorizationService;
        public AuthenticationBasedCustomAuthorizationHandler(IAuthorizationService authorizationService) => _authorizationService = authorizationService;

        public async ValueTask<bool> AuthorizeAsync(AuthorizeCustomContext context)
        {
            var httpContext = context.HttpContext;
            var user = httpContext.User;
            var identity = user.Identity;

            if (identity == null)
                return false;

            if (identity.IsAuthenticated == false)
                return false;

            var cancellationToken = context.CancellationToken;
            var customActivity = context.CustomActivity;
            var policyName = await customActivity.EvaluatePropertyValueAsync(x => x.Policy, cancellationToken);

            if (string.IsNullOrWhiteSpace(policyName))
                return identity.IsAuthenticated;

            var resource = new HttpWorkflowResource(context.WorkflowBlueprint, customActivity.ActivityBlueprint, context.WorkflowInstanceId);
            var authorizationResult = await _authorizationService.AuthorizeAsync(user, resource, policyName);
            return authorizationResult.Succeeded;
        }

    }
}
