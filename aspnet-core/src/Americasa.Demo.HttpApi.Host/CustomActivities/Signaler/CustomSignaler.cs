using Americasa.Demo.CustomActivities.Bookmark;
using Elsa.Activities.Signaling.Models;
using Elsa.Models;
using Elsa.Services;
using Elsa.Services.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Americasa.Demo.CustomActivities.Signaler
{
    public interface ICustomSignaler
    {
        /// <summary>
        /// Runs all workflows that start with or are blocked on the <see cref="SignalCustom"/> activity.
        /// </summary>
        Task<IEnumerable<CollectedWorkflow>> TriggerSignalAsync(string signal, object? input = null, string? workflowInstanceId = null, string? correlationId = null, CancellationToken cancellationToken = default);
    }

    public class CustomSignaler : ICustomSignaler
    {
        private const string? TenantId = default;
        private readonly IWorkflowLaunchpad _workflowLaunchpad;
        public CustomSignaler(IWorkflowLaunchpad workflowLaunchpad) => _workflowLaunchpad = workflowLaunchpad;
        public async Task<IEnumerable<CollectedWorkflow>> TriggerSignalAsync(string signal, object? input = default, string? workflowInstanceId = default, string? correlationId = default, CancellationToken cancellationToken = default)
        {
            var normalizedSignal = signal.ToLowerInvariant();

            return await _workflowLaunchpad.CollectAndExecuteWorkflowsAsync(new WorkflowsQuery(
                nameof(CustomSignal),
                new SignalCustomBookmark { Signal = normalizedSignal },
                correlationId,
                workflowInstanceId,
                default,
                TenantId
            ), new WorkflowInput(new Signal(normalizedSignal, input)), cancellationToken);

        }
    }
}