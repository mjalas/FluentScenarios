using System;
using Xunit.Abstractions;

namespace FluentScenarios
{
    public class Scenario : BaseScenario<Scenario, Action>
    {
        public Scenario(ITestOutputHelper output, object data = null) : base(output, data)
        {
        }

        protected override void AddStep(string name, Action action)
        {
            _steps.Add(new Step<StepAction> {Name = name, Action = new SimpleAction(action)});
        }

        private void AddStep(string name, Table table, Action<Table> action)
        {
            _steps.Add(new Step<StepAction>{Name = name, Action = new TableAction(action, table)});
        }

        public Scenario Given(string statement, Table table, Action<Table> action)
        {
            AddStep($"Given {statement}", table, action);
            _previousStep = PreviousStep.Given;
            return this;
        }
    }
}