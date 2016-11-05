// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistryValueViewModel.cs" company="Compilyator">
//   All rights reserved
// </copyright>
// <summary>
//   Defines the RegistryValueViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WpfApplication1
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Microsoft.Win32;

    using WpfApplication1.Annotations;

    /// <summary>
    /// The registry value.
    /// </summary>
    public class RegistryValueViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The is diff.
        /// </summary>
        private bool isDiff;

        /// <summary>
        /// The kind.
        /// </summary>
        private RegistryValueKind kind;

        /// <summary>
        /// The name.
        /// </summary>
        [CanBeNull]
        private string name;

        /// <summary>
        /// The value.
        /// </summary>
        [CanBeNull]
        private object value;

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether is diff.
        /// </summary>
        public bool IsDiff
        {
            get
            {
                return this.isDiff;
            }

            set
            {
                if (value == this.isDiff)
                {
                    return;
                }

                this.isDiff = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the kind.
        /// </summary>
        public RegistryValueKind Kind
        {
            get
            {
                return this.kind;
            }

            set
            {
                if (value == this.kind)
                {
                    return;
                }

                this.kind = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [CanBeNull]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (value == this.name)
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [CanBeNull]
        public object Value
        {
            get
            {
                return this.value;
            }

            set
            {
                if (RegistryValueViewModel.Equals(value, this.value))
                {
                    return;
                }

                this.value = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CanBeNull] [CallerMemberName] string propertyName = null)
        {
            // ReSharper disable once EventExceptionNotDocumented
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}