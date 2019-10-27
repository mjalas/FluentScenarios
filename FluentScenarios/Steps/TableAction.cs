using System;

namespace FluentScenarios
{
    public class TableAction : StepAction
    {
        private readonly Action<Table> _action;
        private readonly Table _table;

        public TableAction(Action<Table> action, Table table)
        {
            _action = action;
            _table = table;
        }
        
        public void ExecuteAction()
        {
            _action(_table);
        }
    }
}