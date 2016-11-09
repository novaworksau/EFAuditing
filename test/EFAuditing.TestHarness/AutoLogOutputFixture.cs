using EFAuditing.TestHarness.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace EFAuditing.TestHarness
{
    public class AutoLogOutputFixture
    {
        protected IServiceProvider _provider = null;
        protected string _currentUser = null;

        public AutoLogOutputFixture()
        {
            var fixture = new InMemoryFixture();
            _provider = fixture.GetServiceProvider();
            _currentUser = "tsmith"; // Thread.CurrentPrincipal.Identity.Name;
        }

        [Fact, Trait("Category", "A")]
        public void CheckChangeTrackingOutputEntryResult()
        {
            //This test ensures that only the properties without the donotaudit attribute are logged
            using (var db = _provider.GetService<TestDbContext>())
            {
                //Arrange
                db.SeedTestData();
                var customer = db.CustomerNoAuditProperty.First(); //this is a seed collection check EntityFrameworkExtension
                customer.FirstName = "Susan";//FirstName in seed data is John
                customer.LastName = "Smith";
                var modifiedEntry = db.ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).First();

                //Act
                var result = AuditLogBuilder.GetAuditLogs(modifiedEntry, _currentUser, EntityState.Modified);
                var originalValue = ((PropertyLevelAuditLog)result.First()).OriginalValue; //casting to PropertyLevelAuditLog as it's a modification
                var newValue = ((PropertyLevelAuditLog)result.First()).NewValue;
                
                //Assert
                Assert.Equal("John", originalValue);
                Assert.Equal("Susan", newValue);
            }
        }

        [Fact, Trait("Category", "A")]
        public void CheckChangeTrackingOutputEntryResultWithBaseClass()
        {
            //This test ensures that only the properties without the donotaudit attribute are logged
            using (var db = _provider.GetService<TestDbContext>())
            {
                //Arrange
                db.SeedTestData();
                var customer = db.CustomerInheritedFromBaseForAuditing.First(); //this is a seed collection check EntityFrameworkExtension
                customer.FirstName = "Susan";//FirstName in seed data is Toby
                customer.LastName = "Smith";
                var modifiedEntry = db.ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).First();

                //Act
                var result = AuditLogBuilder.GetAuditLogs(modifiedEntry, _currentUser, EntityState.Modified);
                var originalValue = ((PropertyLevelAuditLog)result.First()).OriginalValue; //casting to PropertyLevelAuditLog as it's a modification
                var newValue = ((PropertyLevelAuditLog)result.First()).NewValue;

                //Assert
                Assert.Equal("Toby", originalValue);
                Assert.Equal("Susan", newValue);
            }
        }
    }
}
