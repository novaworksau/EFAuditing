using EFAuditing.TestHarness.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using EFAuditing.TestHarness.TestModel;

namespace EFAuditing.TestHarness
{
    public class DoNotAuditEntityFixture
    {

        protected IServiceProvider _provider = null;
        protected string _currentUser = null;
        

        public DoNotAuditEntityFixture()
        {
            var fixture = new InMemoryFixture();
            _provider = fixture.GetServiceProvider();
            _currentUser = "tsmith"; // Thread.CurrentPrincipal.Identity.Name;
        }

        [Fact]
        public void OneEqualsOne()
        {
            Assert.Equal(1, 1);
        }

        //[Fact]
        //public void DoNotAuditModiedEntryProperty()
        //{
        //    //This test ensures that only the properties without the donotaudit attribute are logged
        //    using (var db = _provider.GetService<TestDbContext>())
        //    {
        //        //Arrange
        //        db.SeedTestData();
        //        var customer = db.CustomerNoAuditProperty.First();
        //        customer.FirstName = "Susan";
        //        customer.LastName = "Smith";//This value should not be logged as it is annotated with [donotlog]
        //        var modifiedEntry = db.ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).First();

        //        //Act
        //        var result = AuditLogBuilder.GetAuditLogs(modifiedEntry, _currentUser, EntityState.Modified);
                
        //        //Assert
        //        Assert.Equal(1, result.Count());//this should pass as 2 values are changed and one has the [donotlog] attribute so it wont be logged
        //    }
        //}

        //[Fact]
        //public void DoNotAuditModifedEntry()
        //{
        //    //This test ensures that only the properties without the donotaudit attribute are logged
        //    using (var db = _provider.GetService<TestDbContext>())
        //    {
        //        //Arrange
        //        db.SeedTestData();
        //        var customer = db.CustomerNoAuditEntity.First();
        //        customer.FirstName = "Susan";
        //        customer.LastName = "Smith";
        //        var modifiedEntry = db.ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).First();

        //        //Act
        //        var result = AuditLogBuilder.GetAuditLogs(modifiedEntry, _currentUser, EntityState.Modified);
                
        //        //Assert
        //        Assert.Equal(0, result.Count());//there should be no results
        //    }
        //}

        //[Fact]
        //public void DoNotAuditModiedEntryWithBase()
        //{
        //    //This test ensures that only the properties without the donotaudit attribute are logged
        //    using (var db = _provider.GetService<TestDbContext>())
        //    {
        //        //Arrange
        //        db.SeedTestData();
        //        var customer = db.CustomerInheritedFromBase.First();
        //        customer.FirstName = "Susan";
        //        customer.LastName = "Smith";
        //        var modifiedEntry = db.ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).First();

        //        //Act
        //        var result = AuditLogBuilder.GetAuditLogs(modifiedEntry, _currentUser, EntityState.Modified);
                
        //        //Assert
        //        Assert.Equal(0, result.Count());//there should be no results
        //    }
        //}

        //[Fact]
        //public void DoNotAuditAddedEntryWithBaseAndDoNotLog()
        //{
        //    //This test ensures that only the properties without the donotaudit attribute are logged
        //    using (var db = _provider.GetService<TestDbContext>())
        //    {
        //        //Arrange
        //        db.SeedTestData();
        //        CustomerInheritedFromBase customer = new CustomerInheritedFromBase { CustomerId = 10, FirstName = "Simon", LastName = "Shaw" };
        //        db.CustomerInheritedFromBase.Add(customer);
        //        var addedEntry = db.ChangeTracker.Entries().Where(p => p.State == EntityState.Added).First();

        //        //Act
        //        var result = AuditLogBuilder.GetAddedAuditLogs(addedEntry, _currentUser, EntityState.Added);
                

        //        //Assert
        //        Assert.Equal(null, result);//there should be no results
        //    }
        //}

        //[Fact]
        //public void DoNotAuditDeletedEntryWithBaseAndDoNotLog()
        //{
        //    //This test ensures that only the properties without the donotaudit attribute are logged
        //    using (var db = _provider.GetService<TestDbContext>())
        //    {
        //        //Arrange
        //        db.SeedTestData();
        //        var customerToBeDeleted = db.CustomerInheritedFromBase.First();
        //        db.CustomerInheritedFromBase.Remove(customerToBeDeleted);
        //        var deletedEntry = db.ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted).First();

        //        //Act
        //        var result = AuditLogBuilder.GetDeletedAuditLogs(deletedEntry, _currentUser, EntityState.Deleted);
        //        if (result != null)
        //            _auditLogList.Add(result);

        //        //Assert
        //        Assert.Equal(0, _auditLogList.Count());//there should be no results
        //    }
        //}
    }
}
