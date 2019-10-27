using System;

namespace FluentScenarios
{
    public class SimpleAction : StepAction
    {
        private readonly Action _action;

        public SimpleAction(Action action)
        {
            _action = action;
        }
        
        public void ExecuteAction()
        {
            _action();
        }
    }
}