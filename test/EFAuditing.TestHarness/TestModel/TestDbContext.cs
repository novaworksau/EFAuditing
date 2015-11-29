using EFAuditing.SampleExtensions;
using EFAuditing.TestHarness;
using EFAuditing.TestHarness.Helpers;
using Microsoft.Data.Entity;

namespace EFAuditing.TestHarness
{
    public class TestDbContext : AuditingDbContext<CustomAuditLog>
    {
        public TestDbContext(IExternalAuditStoreProvider externalAuditStoreProvider, IAuditLogBuilder auditLogBuilder) 
            : base(externalAuditStoreProvider, auditLogBuilder)
        {
            
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
