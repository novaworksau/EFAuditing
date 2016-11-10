using EFAuditing.TestHarness;
using EFAuditing.TestHarness.Helpers;
using EFAuditing.TestHarness.TestModel;
using Microsoft.EntityFrameworkCore;

namespace EFAuditing.TestHarness
{
    public class TestDbContext : AuditingDbContext<AuditLog>
    {
        
        public TestDbContext(IExternalAuditStoreProvider externalAuditStoreProvider, IAuditLogBuilder auditLogBuilder, DbContextOptions options) 
            : base(externalAuditStoreProvider, auditLogBuilder, options)
        {
            
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerNoAuditProperty> CustomerNoAuditProperty { get; set; }

        public DbSet<CustomerNoAuditEntity> CustomerNoAuditEntity { get; set; }

        public DbSet<CustomerInheritedFromBase> CustomerInheritedFromBase { get; set; }

        public DbSet<CustomerInheritedFromBaseForAuditing> CustomerInheritedFromBaseForAuditing { get; set; }
    }
}
