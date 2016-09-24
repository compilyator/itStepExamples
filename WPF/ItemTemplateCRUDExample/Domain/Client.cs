using System;

namespace ItemTemplateCRUDExample.Domain
{
    public class Client
    {
        public Client()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}