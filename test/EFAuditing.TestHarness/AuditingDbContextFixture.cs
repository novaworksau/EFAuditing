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
            _currentUser = "dneimke"; // Thread.CurrentPrincipal.Identity.Name;
        }

        [Fact, Trait("Category", "SanityCheck")]
        public void OneEqualsOne()
        {
            Assert.Equal(1, 1);
        }

        [Fact, Trait("Category", "B")]
        public void ShouldAddData()
        {
            using (var db = _provider.GetService<TestDbContext>())
            {
                // Arrange
                db.SeedTestData();
                var expectedCount = 5;

                //// Act
                var customer = new Customer { CustomerId = 5, FirstName = "Misty", LastName = "Shock" };
                db.Customers.Add(customer);
                db.SaveChanges();
                var actualCount = db.Customers.Count();

                //// Assert
                Assert.Equal(expectedCount, actualCount);
                Assert.True(true);
            }
        }


        [Fact, Trait("Category", "B")]
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


        [Fact, Trait("Category", "B")]
        public void ShouldDeleteData()
        {
            using (var db = _provider.GetService<TestDbContext>())
            {
                // Arrange
                db.SeedTestData();
                var expectedCount = 3;


                // Act
                var cust = db.Customers.FirstOrDefault();
                db.Customers.Remove(cust);
                db.SaveChanges();
                var actualCount = db.Customers.Count();

                // Assert
                Assert.Equal(expectedCount, actualCount);
            }
        }


        [Fact, Trait("Category", "B")]
        public void ShouldCreateAuditLogs()
        {
            using (var db = _provider.GetService<TestDbContext>())
            {
                // Arrange
                var expectedCount = 1;

                // Act
                var customer = new Customer { CustomerId = 5, FirstName = "Misty", LastName = "Shock" };
                db.Customers.Add(customer);
                db.SaveChanges();
                var auditLogs = db.GetAuditLogs().ToList();
                var first = auditLogs.FirstOrDefault() as AuditLog;

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
