using EFAuditing.SampleExtensions;
using EFAuditing.TestHarness.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace EFAuditing.TestHarness
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


        [Fact]
        public void ShouldCreateAuditLogs()
        {
            using (var db = _provider.GetService<TestDbContext>())
            {
                // Arrange
                var expectedCount = 1;

                // Act
                var customer = new Customer { CustomerId = 5, FirstName = "Misty", LastName = "Shock" };
                db.Customers.Add(customer);
                db.SaveChanges(_currentUser);
                var auditLogs = db.GetAuditLogs().ToList();
                var first = auditLogs.FirstOrDefault() as CustomAuditLog;

                // Assert
                Assert.Equal(expectedCount, auditLogs.Count);
                Assert.NotNull(first);

                var differences = first.Differences;
                var diff = JsonConvert.DeserializeObject<List<PropertyDiff>>(differences);

                Assert.Equal(diff.Count, 3); // should be 3 fields -> Id, FirstName, LastName

            }
        }


    }
}
