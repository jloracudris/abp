using Elsa;
using Elsa.Activities.Signaling.Models;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Metadata;
using Elsa.Serialization.Converters;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Americasa.Demo.CustomActivities
{
    [Trigger(
        Category = "Workflows",
        DisplayName = "User Decision Signal",
        Description = "Suspend workflow execution until the specified signal is received.",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class CustomSignal : Activity
    {
        [ActivityInput(Hint = "The name of the signal to wait for.", 
            SupportedSyntaxes = new[] { SyntaxNames.JavaScript, 
                SyntaxNames.Liquid },DefaultValue ="Rules", 
        IsReadOnly =true)]
        public string Signal { get; set; } = "Rules";

        [ActivityInput(Hint = "form json.",
           UIHint = ActivityInputUIHints.MultiLine,
            Category = "Form Rules"
            )]
        public string WorkFlowRules { get; set; } = default!;


        [ActivityInput(Hint = "Input object.",
            UIHint = ActivityInputUIHints.MultiLine,
            Category = "Input"
        )]
        public string Input { get; set; } = default!;

        /// <summary>
        /// Allow authenticated requests only
        /// </summary>
        [ActivityInput(
            Hint = "Check to allow authenticated requests only",
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript, SyntaxNames.Liquid },
            Category = "Security"
        )]
        public bool Authorize { get; set; }

        /// <summary>
        /// Provide a policy to challenge the user with. If the policy fails, the request is forbidden.
        /// </summary>
        [ActivityInput(
            Hint = "Provide a policy to evaluate. If the policy fails, the request is forbidden.",
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript, SyntaxNames.Liquid },
            Category = "Security"
        )]
        public string? Policy { get; set; }



        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TextWriter _writer;

        public CustomSignal(TextWriter writer, IHttpContextAccessor httpContextAccessor)
        {
            _writer = writer;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context) {
            return context.WorkflowExecutionContext.IsFirstPass ? OnResume(context) : Suspend();
            //_writer.WriteLine("Hello World!");
            //return Done();
        }

        protected async override ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            var schemaRules = new List<Workflow>();
            schemaRules.Add(GetWorkFlowRules());
            var rulesEngine = new RulesEngine.RulesEngine(schemaRules.ToArray());

            List<RuleResultTree> response = await rulesEngine.ExecuteAllRulesAsync(schemaRules[0].WorkflowName, GetInputObject());
            await WriteContentAsync(response, context.CancellationToken);
            return Done();
        }

        public Workflow GetWorkFlowRules()
        {
            var jsonWOrkflowRules = JObject.Parse(WorkFlowRules);
            var schemaWorkflow = jsonWOrkflowRules.ToObject<Workflow>();
            return schemaWorkflow;
        }

        public Object GetInputObject()
        {
            var jInput = JObject.Parse(Input);
            return jInput.ToObject<Object>();
        }

        private async Task WriteContentAsync(List<RuleResultTree> contentResult, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext ?? new DefaultHttpContext();
            var response = httpContext.Response;


            if (contentResult == null)
                return;


            var serializerSetting = CreateSerializerSettings();
            var json = JsonConvert.SerializeObject(new {RulesResponse = contentResult }, serializerSetting);
            await response.WriteAsync(json, cancellationToken);
        }

        private JsonSerializerSettings CreateSerializerSettings()
        {
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };

            settings.ContractResolver = new CamelCasePropertyNamesContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = false,
                    ProcessExtensionDataNames = true,
                    OverrideSpecifiedNames = false
                }
            };

            settings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            settings.Converters.Add(new FlagEnumConverter(new DefaultNamingStrategy()));
            settings.Converters.Add(new TypeJsonConverter());
            return settings;
        }
    }
}
