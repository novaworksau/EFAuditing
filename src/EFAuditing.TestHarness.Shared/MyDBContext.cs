using EFAuditing.TestHarness.Shared;
using Microsoft.Data.Entity;

namespace EFAuditing.TestHarness.Shared
{
    public class MyDBContext : AuditingDbContext
    {
        public MyDBContext()
        {
            
        }

        public MyDBContext(IExternalAuditStoreProvider externalAuditStoreProvider) : base(externalAuditStoreProvider)
        {
            
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase();
        }
    }
}
