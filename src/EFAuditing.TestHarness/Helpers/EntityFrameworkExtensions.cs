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
                    new Customer { CustomerId = 1, FirstName = "Ellen", LastName = "Adams" },
                    new Customer { CustomerId = 2, FirstName = "Lisa", LastName = "Andrews" },
                    new Customer { CustomerId = 3, FirstName = "Allen", LastName = "Brewer" },
                    new Customer { CustomerId = 4, FirstName = "Percy", LastName = "Bowman" }
                    );

                dbContext.SaveChanges("dneimke");
            }
        }
    }
}
