using System.Threading;
using Elsa.Services.Models;
using Microsoft.AspNetCore.Http;

namespace Americasa.Demo.CustomActivities.Models
{
    public record AuthorizeCustomContext(HttpContext HttpContext, IActivityBlueprintWrapper<CustomSignal> CustomActivity, IWorkflowBlueprint WorkflowBlueprint, string WorkflowInstanceId, CancellationToken CancellationToken);
}
