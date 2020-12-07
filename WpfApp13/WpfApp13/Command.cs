using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp13
{
   public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public delegate void _delegate(object parametr);
        _delegate _action;
        public Command(_delegate Action)
        {
            _action = Action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
