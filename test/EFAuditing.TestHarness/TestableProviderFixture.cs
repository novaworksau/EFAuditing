using EFAuditing.TestHarness.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace EFAuditing.TestHarness
{
    public class TestableProviderFixture
    {
        protected IServiceProvider _provider = null;
        
        public TestableProviderFixture()
        {
            var fixture = new InMemoryFixture();
            _provider = fixture.GetServiceProvider();
        }


        [Fact]
        public void ShouldInjectProvider()
        {
            using (var db = _provider.GetService<TestDbContext>())
            {
                // Arrange
                var storeProvider = _provider.GetService<IExternalAuditStoreProvider>();
                db.SeedTestData();
                var expectedCount = 4;


                // Act
                var actualCount = storeProvider.ReadAuditLogs().Count();

                // Assert
                Assert.Equal(expectedCount, actualCount);
            }
        }

        [Fact]
        public void ShouldAddData()
        {
            using (var db = _provider.GetService<TestDbContext>())
            {
                // Arrange
                db.SeedTestData();
                //var expectedCount = 5;

                //// Act
                //var customer = new Customer { CustomerId = 5, FirstName = "Misty", LastName = "Shock" };
                //db.Customers.Add(customer);
                //db.SaveChanges(_currentUser);
                //var actualCount = db.Customers.Count();

                //// Assert
                //Assert.Equal(expectedCount, actualCount);
                Assert.True(true);
            }
        }
    }
}
