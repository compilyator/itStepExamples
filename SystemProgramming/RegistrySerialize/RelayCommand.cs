// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelayCommand.cs" company="Compilyator">
//   All rights reserved
// </copyright>
// <summary>
//   Defines the RelayCommand type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WpfApplication1
{
    using System;
    using System.Windows.Input;

    using WpfApplication1.Annotations;

    /// <summary>
    /// The relay command.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// The _can execute.
        /// </summary>
        [CanBeNull]
        private readonly Predicate<object> canExecute;

        /// <summary>
        /// The _execute.
        /// </summary>
        [NotNull]
        private readonly Action<object> execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">
        /// The execute.
        /// </param>
        public RelayCommand([NotNull] Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">
        /// The execute.
        /// </param>
        /// <param name="canExecute">
        /// The can execute.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="execute"/> is <see langword="null"/></exception>
        public RelayCommand([NotNull] Action<object> execute, [CanBeNull] Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// The can execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// The can execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            // ReSharper disable once EventExceptionNotDocumented
            return this.canExecute?.Invoke(parameter) ?? true;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public void Execute(object parameter)
        {
            // ReSharper disable once EventExceptionNotDocumented
            this.execute(parameter);
        }
    }
}