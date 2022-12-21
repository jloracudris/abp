using Elsa;
using Elsa.Activities.Signaling.Models;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Metadata;
using Elsa.Services;
using Elsa.Services.Models;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Americasa.Demo.CustomActivities
{
    [Trigger(
        Category = "Workflows",
        Description = "Suspend workflow execution until the specified signal is received.",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class CustomSignal : Activity, IActivityPropertyOptionsProvider
    {
        [ActivityInput(Hint = "The name of the signal to wait for.", SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid },DefaultValue ="Rules", IsReadOnly =true)]
        public string Signal { get; set; } = "Rules";

        [ActivityInput(Hint = "form json.", SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public string JsonSchema { get; set; } = default!;

        //[ActivityOutput(Hint = "The input that was received with the signal.")]
        //public object? SignalInput { get; set; }

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




        private readonly TextWriter _writer;

       // [ActivityOutput] public object? Output { get; set; }

        //protected override bool OnCanExecute(ActivityExecutionContext context)
        //{
        //    if (context.Input is Signal triggeredSignal)
        //        return string.Equals(triggeredSignal.SignalName, Signal, StringComparison.OrdinalIgnoreCase);

        //    return true;
        //}

        //protected override async ValueTask<bool> OnCanExecuteAsync(ActivityExecutionContext context)
        //{
        //    //if (context.Input is Signal triggeredSignal)
        //    //    return string.Equals(triggeredSignal.SignalName, Signal, StringComparison.OrdinalIgnoreCase);

        //    return true;
        //}

        public CustomSignal(TextWriter writer)
        {
            _writer = writer;
        }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context) {
            return context.WorkflowExecutionContext.IsFirstPass ? OnResume(context) : Suspend();
            //_writer.WriteLine("Hello World!");
            //return Done();
        }

        protected override IActivityExecutionResult OnResume(ActivityExecutionContext context)
        {
            //var triggeredSignal = context.GetInput<Object>()!;
            //SignalInput = triggeredSignal.Input;
            //Output = triggeredSignal.Input;
            // context.LogOutputProperty(this, nameof(Output), Output);
            _writer.WriteLine("Hello World!");
            return Done();
        }

        public object GetOptions(PropertyInfo property)
        {
            throw new NotImplementedException();
        }


        //protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        //{
        //    //var triggeredSignal = context.GetInput<Signal>()!;
        //    //SignalInput = triggeredSignal.Input;
        //    //Output = triggeredSignal.Input;
        //    //context.LogOutputProperty(this, nameof(Output), Output);
        //    await _writer.WriteLineAsync("Hello World!");
        //    return Done();
        //}


    }
}
