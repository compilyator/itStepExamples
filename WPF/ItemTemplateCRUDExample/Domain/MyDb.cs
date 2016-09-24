using System.Data.Entity;

namespace ItemTemplateCRUDExample.Domain
{
    public class MyDb: DbContext
    {
        public MyDb(string connectionName): base(connectionName)
        {
            
        }

        public MyDb()
        {
            
        }

        public DbSet<Client> Clients { get; set; }
        
    }
}