// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistryKeyViewModel.cs" company="Compilyator">
//   All rights reserved
// </copyright>
// <summary>
//   The registry key.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WpfApplication1
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using WpfApplication1.Annotations;

    /// <summary>
    /// The registry key.
    /// </summary>
    public class RegistryKeyViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The diff.
        /// </summary>
        private bool diff;

        /// <summary>
        /// The name.
        /// </summary>
        [CanBeNull]
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryKeyViewModel"/> class.
        /// </summary>
        public RegistryKeyViewModel()
        {
            this.SubKeys = new ObservableCollection<RegistryKeyViewModel>();
            this.Values = new ObservableCollection<RegistryValueViewModel>();
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether diff.
        /// </summary>
        public bool Diff
        {
            get
            {
                return this.diff;
            }

            set
            {
                if (value == this.diff)
                {
                    return;
                }

                this.diff = value;
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
        /// Gets or sets the sub keys.
        /// </summary>
        [NotNull]
        public ObservableCollection<RegistryKeyViewModel> SubKeys { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        [NotNull]
        public ObservableCollection<RegistryValueViewModel> Values { get; set; }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] [CanBeNull] string propertyName = null)
        {
            // ReSharper disable once EventExceptionNotDocumented
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}