using EFAuditing.TestHarness.Shared.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace EFAuditing.TestHarness.Shared
{
    public class AuditingDbContextFixture
    {
        protected IServiceProvider _provider = null;
        protected string _currentUser = null;

        public AuditingDbContextFixture()
        {
            var fixture = new InMemoryFixture();
            _provider = fixture.GetServiceProvider();
            _currentUser = Thread.CurrentPrincipal.Identity.Name;
        }


        


        [Fact]
        public void ShouldAddData()
        {
            using (var db = _provider.GetService<TestDbContext>())
            {
                // Arrange
                db.SeedTestData();
                var expectedCount = 5;

                // Act
                var customer = new Customer { CustomerId = 5, FirstName = "Misty", LastName = "Shock" };
                db.Customers.Add(customer);
                db.SaveChanges(_currentUser);
                var actualCount = db.Customers.Count();

                // Assert
                Assert.Equal(expectedCount, actualCount);
            }
        }


        [Fact]
        public void ShouldRetrieveData()
        {
            using (var db = _provider.GetService<TestDbContext>())
            {
                // Arrange
                db.SeedTestData();
                var expectedCount = 4;


                // Act
                var actualCount = db.Customers.Count();

                // Assert
                Assert.Equal(expectedCount, actualCount);
            }
        }


        [Fact]
        public void ShoulDeleteData()
        {
            using (var db = _provider.GetService<TestDbContext>())
            {
                // Arrange
                db.SeedTestData();
                var expectedCount = 3;


                // Act
                var cust = db.Customers.FirstOrDefault();
                db.Customers.Remove(cust);
                db.SaveChanges(_currentUser);
                var actualCount = db.Customers.Count();

                // Assert
                Assert.Equal(expectedCount, actualCount);
            }
        }
    }
}
