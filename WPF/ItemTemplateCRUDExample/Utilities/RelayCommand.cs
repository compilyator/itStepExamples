using System;
using System.Windows.Input;

namespace ItemTemplateCRUDExample.Utilities
{
    public class RelayCommand : ICommand
    {
        #region Fields
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        #endregion

        // Fields
        #region Constructors

        /// <summary>
        /// Create instance of <see cref="RelayCommand"/>
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<object> execute) : this(execute, null) { }

        /// <summary>
        /// Create instance of <see cref="RelayCommand"/>
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <exception cref="ArgumentNullException"><paramref name="execute"/> is <see langword="null" />.</exception>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion Constructors

        #region ICommand Members

        /// <summary>
        /// Can execute implementation
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Can execute changed event
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Execute implementation
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }

        #endregion
        // ICommand Members
    }
}