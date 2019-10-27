using System;

namespace FluentScenarios
{
    public class ValueAction : StepAction
    {
        private readonly Action<dynamic> _action;
        private readonly dynamic _value;

        public ValueAction(Action<dynamic> action, dynamic value)
        {
            _action = action;
            _value = value;
        }
        
        public void ExecuteAction()
        {
            _action(_value);
        }
    }
}