﻿// ReSharper disable RedundantUsingDirective
using System;
using System.Reflection;
using System.Linq;
using EFAuditing.TestHarness.Shared;
using Microsoft.Data.Entity.Metadata;

namespace EFAuditing.TestHarness
{
    public class Program
    {
        public void Main(string[] args)
        {
            using (var myDbContext = new TestDbContext())
            {
                myDbContext.Database.EnsureDeleted();
                myDbContext.Database.EnsureCreated();

                var customer = new Customer()
                {
                    FirstName = "TestFirstName",
                    LastName = "TEstLAstNAme"
                };
                myDbContext.Customers.Add(customer);

                var auditablePropCount =
                    customer.GetType()
                        .GetProperties()
                        .Count(p => !p.GetCustomAttributes(typeof (DoNotAudit), true).Any());

                var nonAuditablePropCount =
                    customer.GetType()
                        .GetProperties()
                        .Count(p => p.GetCustomAttributes(typeof(DoNotAudit), true).Any());
                myDbContext.SaveChanges("Test User");

                customer.LastName = "TestLastName"; // This should throw an exception below
                myDbContext.SaveChanges("Test User");

                Console.WriteLine($"Added object with {auditablePropCount} auditable properties and {nonAuditablePropCount} non-auditable properties." );

                var auditLogs = myDbContext.GetAuditLogs().ToList();
                Console.WriteLine($"Audit log contains {auditLogs.Count()} entries.");
                foreach (var auditLog in myDbContext.GetAuditLogs())
                {
                    Console.WriteLine($"AuditLogId:{auditLog.AuditLogId} EntityName:{auditLog.EntityName} Differences:{auditLog.Differences} EventDateTime:{auditLog.EventDateTime}");
                }

                if (auditLogs.Count() == auditablePropCount)
                    Console.WriteLine("Test succeeded.");
                else
                    throw new Exception("Something is wrong.");

                Console.Read();
                myDbContext.Database.EnsureDeleted();
            }
        }
    }
}
