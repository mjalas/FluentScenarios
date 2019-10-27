using System;

namespace FluentScenarios
{
    public class ScenarioFailed : Exception
    {
        public ScenarioFailed(string message) : base(message: message) { }
    }
}