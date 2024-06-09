using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetApp.Infrastructure.Command.Base;

namespace BudgetApp.Infrastructure.Command
{
    class RelayCommand : CommandBase
    {
        private readonly Action<object> _action;
        private readonly Func<object, bool> _canExecuteAction;

        public RelayCommand(Action<object> action, Func<object, bool> canExecuteAction = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            _action = action;
            _canExecuteAction = canExecuteAction;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecuteAction?.Invoke(parameter) ?? true;
        }

        public override void Execute(object parameter)
        {
            _action?.Invoke(parameter);
        }
    }
}
