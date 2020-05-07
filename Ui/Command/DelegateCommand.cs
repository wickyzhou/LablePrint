using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Ui.Command
{
    public class DelegateCommand : ICommand
    {

        public delegate void RefreshParentDelegate();

        public Action<object> ExecuteAction { get; set; }

        public Func<object, bool> CanExecuteFunc { get; set; }

       

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (this.CanExecuteFunc == null)
            {
                return true;
            }
            return this.CanExecuteFunc(parameter);
        }

        public void Execute(object parameter)
        {
            if (this.ExecuteAction == null)
            {
                return;
            }
            this.ExecuteAction(parameter);
        }



        //readonly Action<object> _execute;
        //readonly Predicate<object> _canExecute;

        //public DelegateCommand(Action<object> execute)
        //    : this(execute, null)
        //{ }
        //public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        //{ if (execute == null) throw new ArgumentNullException("execute"); _execute = execute; _canExecute = canExecute; }


        //public bool CanExecute(object parameter)
        //{ return _canExecute == null ? true : _canExecute(parameter); }
        //public event EventHandler CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}
        //public void Execute(object parameter)
        //{
        //    _execute(parameter);
        //}
    }
}
