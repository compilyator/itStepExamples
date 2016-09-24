using System.Collections.Generic;
using System.Collections.ObjectModel;
using ItemTemplateCRUDExample.Annotations;

namespace ItemTemplateCRUDExample.ViewModels
{
    public class ClientsCollection : ObservableCollection<ClientViewModel>
    {
        public ClientsCollection()
        {
        }

        public ClientsCollection(List<ClientViewModel> list) : base(list)
        {
        }

        public ClientsCollection([NotNull] IEnumerable<ClientViewModel> collection) : base(collection)
        {
        }
    }
}