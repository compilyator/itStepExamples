// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistryViewModel.cs" company="Compilyator">
//   All rights reserved
// </copyright>
// <summary>
//   The registry view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WpfApplication1
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using Microsoft.Win32;

    using Newtonsoft.Json;

    using WpfApplication1.Annotations;

    /// <summary>
    /// The registry view model.
    /// </summary>
    public class RegistryViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The path.
        /// </summary>
        [CanBeNull]
        private string path;

        /// <summary>
        /// The snapshot.
        /// </summary>
        [CanBeNull]
        private RegistryKeyViewModel snapshot;

        /// <summary>
        /// The verified snapshot.
        /// </summary>
        [CanBeNull]
        private RegistryKeyViewModel verifiedSnapshot;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryViewModel"/> class.
        /// </summary>
        public RegistryViewModel()
        {
            this.Path = string.Empty;
            this.CreateSnapshotCommand = new RelayCommand(this.CreateSnapshot, o => !string.IsNullOrEmpty(o?.ToString()));
            this.VerifySnapshotCommand = new RelayCommand(this.VerifySnapshot, o => !string.IsNullOrEmpty(o?.ToString()));
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the create snapshot command.
        /// </summary>
        [NotNull]
        public ICommand CreateSnapshotCommand { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        [NotNull]
        public string Path
        {
            get
            {
                return this.path ?? string.Empty;
            }

            set
            {
                if (value == this.path)
                {
                    return;
                }

                this.path = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the snapshot.
        /// </summary>
        [CanBeNull]
        public RegistryKeyViewModel Snapshot
        {
            get
            {
                return this.snapshot;
            }

            set
            {
                if (object.Equals(value, this.snapshot))
                {
                    return;
                }

                this.snapshot = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.SnapshotJson));
            }
        }

        /// <summary>
        /// The snapshot in JSON.
        /// </summary>
        [NotNull]
        public string SnapshotJson => JsonConvert.SerializeObject(this.Snapshot ?? new object(), Formatting.Indented);

        /// <summary>
        /// The verified snapshot in JSON.
        /// </summary>
        [NotNull]
        public string VerifiedSnapshotJson => JsonConvert.SerializeObject(this.VerifiedSnapshot ?? new object(), Formatting.Indented);

        /// <summary>
        /// Gets or sets the verified snapshot.
        /// </summary>
        [CanBeNull]
        public RegistryKeyViewModel VerifiedSnapshot
        {
            get
            {
                return this.verifiedSnapshot;
            }

            set
            {
                if (object.Equals(value, this.verifiedSnapshot))
                {
                    return;
                }

                this.verifiedSnapshot = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.VerifiedSnapshotJson));
            }
        }

        /// <summary>
        /// Gets or sets the create snapshot command.
        /// </summary>
        [NotNull]
        public ICommand VerifySnapshotCommand { get; set; }

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

        /// <summary>
        /// The validate.
        /// </summary>
        /// <param name="snapshot">
        /// The snapshot.
        /// </param>
        /// <param name="verifiedSnapshot">
        /// The verified snapshot.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Both parameters are null
        /// </exception>
        [MustUseReturnValue]
        private static bool Validate(
            [CanBeNull] RegistryKeyViewModel snapshot,
            [CanBeNull] RegistryKeyViewModel verifiedSnapshot)
        {
            if (snapshot == null)
            {
                if (verifiedSnapshot != null)
                {
                    verifiedSnapshot.Diff = true;
                    return true;
                }

                throw new InvalidOperationException("Both parameters are null");
            }

            if (verifiedSnapshot == null)
            {
                snapshot.Diff = true;
                return true;
            }

            ValidateValues(snapshot, verifiedSnapshot);

            ValidateSubKeys(snapshot, verifiedSnapshot);

            return snapshot.Diff || verifiedSnapshot.Diff;
        }

        /// <summary>
        /// The validate sub keys.
        /// </summary>
        /// <param name="snapshot">
        /// The snapshot.
        /// </param>
        /// <param name="verifiedSnapshot">
        /// The verified snapshot.
        /// </param>
        private static void ValidateSubKeys(
            [NotNull] RegistryKeyViewModel snapshot,
            [NotNull] RegistryKeyViewModel verifiedSnapshot)
        {
            foreach (var verifiedSnapshotSubKey in verifiedSnapshot.SubKeys)
            {
                var snapshotSubKey = snapshot.SubKeys.SingleOrDefault(e => e.Name == verifiedSnapshotSubKey.Name);
                if (snapshotSubKey == null)
                {
                    verifiedSnapshotSubKey.Diff = true;
                }
                else
                {
                    snapshot.Diff = Validate(snapshotSubKey, verifiedSnapshotSubKey);
                }
            }
        }

        /// <summary>
        /// The validate values.
        /// </summary>
        /// <param name="snapshot">
        /// The snapshot.
        /// </param>
        /// <param name="verifiedSnapshot">
        /// The verified snapshot.
        /// </param>
        private static void ValidateValues(
            [NotNull] RegistryKeyViewModel snapshot,
            [NotNull] RegistryKeyViewModel verifiedSnapshot)
        {
            foreach (var verifiedSnapshotValue in verifiedSnapshot.Values)
            {
                var snapshotValue = snapshot.Values.SingleOrDefault(e => e.Name == verifiedSnapshotValue.Name);

                // ReSharper disable once ComplexConditionExpression
                if (snapshotValue == null ||
                    snapshotValue.Kind != verifiedSnapshotValue.Kind ||
                    snapshotValue.Value == null ||
                    !snapshotValue.Value.Equals(verifiedSnapshotValue.Value))
                {
                    verifiedSnapshotValue.IsDiff = true;
                    snapshot.Diff = true;
                    verifiedSnapshot.Diff = true;
                }
            }
        }

        /// <summary>
        /// The create snapshot.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        private void CreateSnapshot([NotNull] object parameter)
        {
            var parameterPath = parameter.ToString();
            var key = Registry.CurrentUser.OpenSubKey(parameterPath);
            if (key == null)
            {
                return;
            }

            this.Snapshot = this.SnapKey(key);
        }

        /// <summary>
        /// The snap key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryKeyViewModel"/>.
        /// </returns>
        [NotNull]
        private RegistryKeyViewModel SnapKey([NotNull] RegistryKey key)
        {
            var result = new RegistryKeyViewModel { Name = System.IO.Path.GetFileName(key.Name) };
            foreach (var valueName in key.GetValueNames())
            {
                result.Values.Add(
                    new RegistryValueViewModel
                    {
                        Name = valueName,
                        Kind = key.GetValueKind(valueName),
                        Value = key.GetValue(valueName)
                    });
            }

            foreach (var subKeyName in key.GetSubKeyNames())
            {
                var subKey = key.OpenSubKey(subKeyName);
                Debug.Assert(subKey != null, "subKey != null");
                result.SubKeys.Add(this.SnapKey(subKey));
            }

            return result;
        }

        /// <summary>
        /// The create snapshot.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        private void VerifySnapshot([NotNull] object parameter)
        {
            var parameterPath = parameter.ToString();
            var key = Registry.CurrentUser.OpenSubKey(parameterPath);
            if (key == null)
            {
                return;
            }

            this.VerifiedSnapshot = this.SnapKey(key);
            this.VerifiedSnapshot.Diff = Validate(this.Snapshot, this.VerifiedSnapshot);
            this.OnPropertyChanged(nameof(this.VerifiedSnapshotJson));
        }
    }
}