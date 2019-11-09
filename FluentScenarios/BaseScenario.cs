using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace FluentScenarios
{
   public abstract class BaseScenario<T, TAction>
    where T : BaseScenario<T, TAction>
    {
        protected ITestOutputHelper _output;

        protected List<string> _outputContent;

        protected const string PASSED = "\u2714";
        protected const string FAILED = "\u274C";

        protected readonly List<Step<StepAction>> Steps;
        protected readonly Dictionary<string, string> Statements;

        protected enum PreviousStep
        {
            Given,
            When,
            Then
        }

        protected PreviousStep PreviousStepValue;
        protected readonly ScenarioContext Context;

        protected BaseScenario(ITestOutputHelper output, object data = null)
        {
            _output = output;
            _output = output;
            _outputContent = new List<string>();
            PreviousStepValue = PreviousStep.Given;
            Steps = new List<Step<StepAction>>();
            Statements = new Dictionary<string, string>();
            if (data != null)
            {
                Context = new ScenarioContext{Data = data};
            }
        }

        protected abstract void AddStep(string name, TAction action);

        public T Given(string statement, TAction stepAction)
        {
            AddStep($"Given {statement}", stepAction);
            PreviousStepValue = PreviousStep.Given;
            return (T) this;
        }

        public T When(string statement, TAction stepAction)
        {
            AddStep($"When {statement}", stepAction);
            PreviousStepValue = PreviousStep.When;
            return (T) this;
        }

        public T Then(string statement, TAction stepAction)
        {
            AddStep($"Then {statement}", stepAction);
            PreviousStepValue = PreviousStep.Then;
            return (T) this;
        }

        public T And(string statement, TAction stepAction)
        {
            var name = $"{Enum.GetName(typeof(PreviousStep), PreviousStepValue)} {statement}";

            AddStep(name, stepAction);
            return (T) this;
        }

        protected virtual void AddStepResult(string stepName, string marker)
        {
            var step = Statements[stepName];
            _outputContent.Add($"{step}  {marker}");
        }

        protected (string name, string exception, string message, string stackTrace) GetFailureContent(
            Step<StepAction> step, Exception ex)
        {
            return (name: step.Name, exception: ex.GetType().ToString(),
                message: ex.Message, stackTrace: ex.StackTrace);
        }
        
        public void Run()
        {
            bool failed = false;
            (string name, string exception, string message, string stackTrace) failureContent = ("", "", "", "");
            foreach(var step in Steps)
            {
                try
                {
                    step.Action.ExecuteAction();
                    AddStepResult(step.Name, PASSED);
                }
                catch (XunitException ex)
                {
                    AddStepResult(step.Name, FAILED);
                    failed = true;
                    failureContent = GetFailureContent(step, ex);
                    break;
                }
            }

            if (failed)
            {
                throw new ScenarioFailed($"Step: {failureContent.name}\nThrew: {failureContent.exception}\nMessage: {failureContent.message}\n\nStackTrace:\n{failureContent.stackTrace}");
            }
            
            ShowTestResult();
        }

        public void ShowTestResult()
        {
            _output.WriteLine(String.Join("\n", _outputContent) + "\n");
        }
    }
}