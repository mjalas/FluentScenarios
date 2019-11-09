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

        private static string GenerateTableString(Table table)
        {
            var output = new List<string>();
            var firstRow = true;
            var separatorLine = "";
            foreach (var row in table.Rows)
            {
                switch (row)
                {
                    case IDictionary<string, object> rowAsDictionary:
                    {
                        var rowAsString = "|";
                        if (firstRow)
                        {
                            rowAsString = rowAsDictionary.Aggregate(rowAsString, (current, entry) => 
                                $"{current}\t{entry.Key}\t|");
                            rowAsString += "\n";
                            var separator = new String('-', rowAsString.Length+2);
                            separatorLine = $"+{separator}+";
                            rowAsString += $"{separatorLine}\n|";
                            
                            output.Add(separatorLine);
                            firstRow = false;
                        }
                        rowAsString = rowAsDictionary.Aggregate(rowAsString, (current, entry) =>
                            $"{current}\t{entry.Value}\t|");
                        output.Add(rowAsString);
                        break;
                    }
                }
            }
            output.Add(separatorLine);
            return string.Join("\n", output);
        }

        private void AddStep(string name, Table table, Action<Table> action)
        {
            var tableString = GenerateTableString(table);
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