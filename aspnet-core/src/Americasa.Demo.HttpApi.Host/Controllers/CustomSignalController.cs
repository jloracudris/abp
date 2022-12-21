using Americasa.Demo.CustomActivities.Signaler;
using Elsa.Server.Api.Endpoints.Signals;
using Elsa.Server.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Open.Linq.AsyncExtensions;
using System.Linq;
using Elsa.Services.Models;

namespace Americasa.Demo.Controllers
{
    [ApiController]
    [Route("custom-signals/{signalName}/execute")]
    [Produces("application/json")]
    public class CustomSignalController : Controller
    {
        private readonly ICustomSignaler _signaler;
        private readonly IEndpointContentSerializerSettingsProvider _serializerSettingsProvider;

        public CustomSignalController(ICustomSignaler signaler, IEndpointContentSerializerSettingsProvider serializerSettingsProvider)
        {
            _signaler = signaler;
            _serializerSettingsProvider = serializerSettingsProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Handle(string signalName, ExecuteSignalRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _signaler.TriggerSignalAsync(signalName, request.Input, request.WorkflowInstanceId, request.CorrelationId, cancellationToken);
            if (Response.HasStarted)
                return new EmptyResult();

            return Json(
                new ExecuteSignalResponse(result.ToList().Select(x => new CollectedWorkflow(x.WorkflowInstanceId,null, x.ActivityId)).ToList()),
                _serializerSettingsProvider.GetSettings());
        }
    }
}
