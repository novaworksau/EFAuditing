using EFAuditing.TestHarness;
using Microsoft.Data.Entity;

namespace EFAuditing.TestHarness
{
    public class TestDbContext : AuditingDbContext
    {
        public TestDbContext()
        {
            
        }

        public TestDbContext(IExternalAuditStoreProvider externalAuditStoreProvider) : base(externalAuditStoreProvider)
        {
            
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
