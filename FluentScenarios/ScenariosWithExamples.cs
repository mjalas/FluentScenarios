using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.RegularExpressions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace FluentScenarios
{
    public class ScenarioWithExamples : BaseScenario<ScenarioWithExamples, Action<dynamic>>
    {
        private readonly Dictionary<string, string> _statementsWithKeyChangedToValue;

        public ScenarioWithExamples(ITestOutputHelper output, object data = null) : base(output, data)
        {
            _statementsWithKeyChangedToValue = new Dictionary<string, string>();
        }


        private dynamic GetValuesFromStatement(string statement)
        {
            dynamic obj = new ExpandoObject();
            var objAsDict = obj as IDictionary<string, object>;
            var match = Regex.Match(statement, @"\@[A-Za-z0-9]+");
            var counter = 0;

            while (match.Success)
            {
                if (counter >= 10) // Safety break that can be removed when confidence in the algorithm is gained.
                {
                    break;
                }

                var name = match.Value.Replace("@", "");
                var value = _context.Data.GetType().GetProperty(name).GetValue(_context.Data, null);
                objAsDict[name] = value;
                var updatedStatement = statement.Replace(match.Value, value.ToString());
                _statementsWithKeyChangedToValue.Add(statement, updatedStatement);
                match = match.NextMatch();
                counter++;
            }

            if (counter == 0)
            {
                _statementsWithKeyChangedToValue.Add(statement, statement);
            }

            return obj;
        }

        protected override void AddStep(string name, Action<dynamic> action)
        {
            var stepValues = GetValuesFromStatement(name);
            _steps.Add(new Step<StepAction> {Name = name, Action = new ValueAction(action, stepValues)});
        }

        protected override void AddStepResult(string stepName, string marker)
        {
            var step = _statementsWithKeyChangedToValue[stepName];
            _outputContent.Add($"{step}  {marker}");
        }

    }
}