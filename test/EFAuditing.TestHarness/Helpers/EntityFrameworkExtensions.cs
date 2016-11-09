using EFAuditing.TestHarness.TestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFAuditing.TestHarness.Helpers
{
    public static class EntityFrameworkExtensions
    {
        public static void SeedTestData(this TestDbContext dbContext)
        {
            if (!dbContext.Customers.Any())
            {
                // refer: https://en.wikipedia.org/wiki/Contoso#Contoso_employees
                dbContext.Customers.AddRange(
                    new Customer { FirstName = "Ellen", LastName = "Adams" },
                    new Customer { FirstName = "Lisa", LastName = "Andrews" },
                    new Customer { FirstName = "Allen", LastName = "Brewer" },
                    new Customer { FirstName = "Percy", LastName = "Bowman" }
                    );

                dbContext.SaveChanges();
            }

            if (!dbContext.CustomerNoAuditEntity.Any())
            {
                // refer: https://en.wikipedia.org/wiki/Contoso#Contoso_employees
                dbContext.CustomerNoAuditEntity.AddRange(
                    new CustomerNoAuditEntity { FirstName = "Trevor", LastName = "Smith" },
                    new CustomerNoAuditEntity { FirstName = "Darren", LastName = "Neimke" }
                    );

                dbContext.SaveChanges();
            }

            if (!dbContext.CustomerNoAuditProperty.Any())
            {
                // refer: https://en.wikipedia.org/wiki/Contoso#Contoso_employees
                dbContext.CustomerNoAuditProperty.AddRange(
                    new CustomerNoAuditProperty { FirstName = "John", LastName = "Ruckert" },
                    new CustomerNoAuditProperty { FirstName = "Ben", LastName = "Thies" }
                    );

                dbContext.SaveChanges();
            }

            if (!dbContext.CustomerInheritedFromBase.Any())
            {
                // refer: https://en.wikipedia.org/wiki/Contoso#Contoso_employees
                dbContext.CustomerInheritedFromBase.AddRange(
                    new CustomerInheritedFromBase { FirstName = "Toby", LastName = "Shallis" },
                    new CustomerInheritedFromBase { FirstName = "Ravi", LastName = "Jones" }
                    );

                dbContext.SaveChanges();
            }
        }
    }
}
