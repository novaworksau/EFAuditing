using EFAuditing.TestHarness;
using EFAuditing.TestHarness.Helpers;
using Microsoft.Data.Entity;

namespace EFAuditing.TestHarness
{
    public class TestDbContext : AuditingDbContext
    {
        public TestDbContext()
            : base(new TestableDbAuditStoreProvider())
        {
            
        }

        public TestDbContext(IExternalAuditStoreProvider externalAuditStoreProvider) : base(externalAuditStoreProvider)
        {
            
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
