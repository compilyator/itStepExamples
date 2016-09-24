using ItemTemplateCRUDExample.Domain;
using ItemTemplateCRUDExample.ViewModels;

namespace ItemTemplateCRUDExample.Utilities
{
    public static class ClientConverter
    {
        public static ClientViewModel ToViewModel(this Client client)
        {
            return new ClientViewModel
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                DateOfBirth = client.DateOfBirth
            };
        }

        public static void UpdateClient(this ClientViewModel viewModel, MyDb context)
        {
            var client = context.Clients.Find(viewModel.Id);
            client.FirstName = viewModel.FirstName;
            client.LastName = viewModel.LastName;
            client.DateOfBirth = viewModel.DateOfBirth;
        }
    }
}
