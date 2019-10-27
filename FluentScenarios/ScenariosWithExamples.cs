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

        public ScenarioWithExamples(ITestOutputHelper output, object data = null) : base(output, data)
        {
        }


        private dynamic GetValuesFromStatement(string statement)
        {
            dynamic obj = new ExpandoObject();
            var objAsDict = obj as IDictionary<String, object>;
            Match match = Regex.Match(statement, @"\@[A-Za-z0-9]+");
            int counter = 0;

            while (match.Success)
            {
                if (counter >= 10) // Safety break that can be removed when confidence in the algorithm is gained.
                {
                    break;
                }

                var name = match.Value.Replace("@", "");
                var value = _context.Data.GetType().GetProperty(name).GetValue(_context.Data, null);
                objAsDict[name] = value;
                match = match.NextMatch();
                counter++;
            }

            return obj;
        }

        protected override void AddStep(string name, Action<dynamic> action)
        {
            var stepValues = GetValuesFromStatement(name);
            _steps.Add(new Step<StepAction> {Name = name, Action = new ValueAction(action, stepValues)});
        }

    }
}