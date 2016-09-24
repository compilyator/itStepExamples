using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ItemTemplateCRUDExample.Annotations;
using ItemTemplateCRUDExample.Domain;
using ItemTemplateCRUDExample.Utilities;

namespace ItemTemplateCRUDExample.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly MyDb _context;
        private ClientViewModel _selectedClient;
        private bool _allowEdit;

        public MainViewModel() { }

        public MainViewModel(string connectionName = "name=MyDb")
        {
            _context = new MyDb(connectionName);
            _context.Clients.Load();
            Clients = new ClientsCollection(_context.Clients.Local.Select(client => client.ToViewModel()));
            DeleteCommand = new RelayCommand(o => Delete(o as ClientViewModel));
            CreateCommand = new RelayCommand(o => Create());
            SaveChangesCommand = new RelayCommand(o => SaveChanges());
        }

        public ClientsCollection Clients { get; set; }

        public ClientViewModel SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                if (Equals(value, _selectedClient)) return;
                _selectedClient = value;
                AllowEdit = false;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCurrentNotNull));
            }
        }

        public ICommand DeleteCommand { get; set; }

        public ICommand CreateCommand { get; set; }

        public ICommand SaveChangesCommand { get; set; }

        public bool AllowEdit
        {
            get { return _allowEdit; }
            set
            {
                if (value == _allowEdit) return;
                _allowEdit = value;
                OnPropertyChanged();
            }
        }

        public bool IsCurrentNotNull => SelectedClient != null;

        private void Delete(ClientViewModel clientViewModel)
        {
            if (clientViewModel == null) return;
            var client = _context.Clients
                .SingleOrDefault(e => e.Id == clientViewModel.Id);
            _context.Clients.Remove(client);
            Clients.Remove(clientViewModel);
        }

        private void Create()
        {
            var client = new Client
            {
                DateOfBirth = DateTime.Now
            };
            _context.Clients.Add(client);
            var clientViewModel = client.ToViewModel();
            Clients.Add(clientViewModel);
            SelectedClient = clientViewModel;
            AllowEdit = true;
        }

        private void SaveChanges()
        {
            Clients.ForEach(viewModel => viewModel.UpdateClient(_context));
            _context.SaveChanges();
        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}