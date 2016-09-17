using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ConvertersDemo.Annotations;

namespace ConvertersDemo
{
    public class ViewModel: INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private bool _agree;

        public ViewModel()
        {
            FirstName = String.Empty;
            LastName = String.Empty;
            ClickCommand = new RelayCommand(
                o =>
                {
                    MessageBox.Show($"User {FirstName} {LastName} successfully registered!");
                }
                , o => o is bool && (bool) o
                );
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (value == _firstName) return;
                _firstName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AllFieldsRight));
                OnPropertyChanged(nameof(IsBill));
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (value == _lastName) return;
                _lastName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AllFieldsRight));
            }
        }

        public bool Agree
        {
            get { return _agree; }
            set
            {
                if (value == _agree) return;
                _agree = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AllFieldsRight));
            }
        }

        public bool AllFieldsRight => !FirstName.IsDirty() && !LastName.IsDirty() && Agree;

        public bool IsBill => FirstName.Equals("Bill");

        public RelayCommand ClickCommand { get; set; }

        #region INotifyPropertyChanged 

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}