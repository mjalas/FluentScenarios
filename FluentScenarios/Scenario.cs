using System;
using System.Collections.Generic;
using System.Linq;
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
            Statements.Add(name, $"{name} @resultMarker");
            Steps.Add(new Step<StepAction> {Name = name, Action = new SimpleAction(action)});
        }

        private void AddStep(string name, Table table, Action<Table> action)
        {
            var tableString = TableStringBuilder.Build(table);
            var formattedStatement = $"{name} @resultMarker\n{tableString}";
            Statements.Add(name, formattedStatement);
            Steps.Add(new Step<StepAction>{Name = name, Action = new TableAction(action, table)});
        }

        public Scenario Given(string statement, Table table, Action<Table> action)
        {
            AddStep($"Given {statement}", table, action);
            PreviousStepValue = PreviousStep.Given;
            return this;
        }

        protected override void AddStepResult(string stepName, string marker)
        {
            var step = Statements[stepName];
            step = step.Replace("@resultMarker", marker);
            _outputContent.Add(step);
        }
    }
}