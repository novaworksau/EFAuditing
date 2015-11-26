using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// ReSharper disable once RedundantUsingDirective Since GetProperties Need this for dnxcore50
using System.Reflection;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;
using Microsoft.Data.Entity.Metadata;
using System.Runtime.InteropServices;

namespace EFAuditing.SampleExtensions
{
    /// <summary>
    /// The AuditLogBuilder class is used internally by AuditingDbContext.
    /// </summary>
    public class CustomAuditLogBuilder : IAuditLogBuilder
    {
        private const string KeySeperator = ";";
        [DllImport("rpcrt4.dll", SetLastError = true)]
        static extern int UuidCreateSequential(out Guid guid);

        public List<AuditLog> GetAuditLogsForExistingEntities(string userName,
            IEnumerable<EntityEntry> modifiedEntityEntries,
            IEnumerable<EntityEntry> deletedEntityEntries)
        {
            var auditLogs = new List<AuditLog>();
            foreach (var auditRecordsForEntityEntry in modifiedEntityEntries.Select(
                        changedEntity => GetAuditLogs(changedEntity, userName, EntityState.Modified)))
            { 
                auditLogs.AddRange(auditRecordsForEntityEntry);
            }

            foreach (var auditRecordsForEntityEntry in deletedEntityEntries.Select(
                        changedEntity => GetAuditLogs(changedEntity, userName, EntityState.Deleted)))
            { 
                auditLogs.AddRange(auditRecordsForEntityEntry);
            }

            return auditLogs;
        }

        public List<AuditLog> GetAuditLogsForAddedEntities(string userName,
            IEnumerable<EntityEntry> addedEntityEntries)
        {
            var auditLogs = new List<AuditLog>();
            foreach (var auditRecordsForEntityEntry in addedEntityEntries.Select(
                        changedEntity => GetAuditLogs(changedEntity, userName, EntityState.Added)))
            { 
                auditLogs.AddRange(auditRecordsForEntityEntry);
            }

            return auditLogs;
        }

        private static IEnumerable<CustomAuditLog> GetAuditLogs(EntityEntry entityEntry, string userName, EntityState entityState)
        {
            var returnValue = new List<CustomAuditLog>();
            // var keyRepresentation = BuildKeyRepresentation(entityEntry, KeySeperator);
            DateTime RightNow = DateTime.Now;
            Guid AuditBatchID;
            UuidCreateSequential(out AuditBatchID);

            var auditedPropertyNames =
                entityEntry.Entity.GetType()
                    .GetProperties().Where(p => !p.GetCustomAttributes(typeof(DoNotAudit), true).Any())
                    .Select(info => info.Name);

            var differences = new List<Tuple<string, string, string>>();


            foreach (var propertyEntry in entityEntry.Metadata.GetProperties()
                .Where(x => auditedPropertyNames.Contains(x.Name))
                .Select(property => new { PropertyName = property.Name, Property = entityEntry.Property(property.Name) }))
            {
                if (entityState == EntityState.Modified)
                {
                    var originalValue = Convert.ToString(propertyEntry.Property.OriginalValue);
                    var currentValue = Convert.ToString(propertyEntry.Property.CurrentValue);

                    if (originalValue == currentValue) //Values are the same, don't log
                        continue;

                    differences.Add(new Tuple<string, string, string>(propertyEntry.PropertyName, originalValue, currentValue));
                }
            }


            returnValue.Add(new CustomAuditLog
            {
                Differences = differences,
                EventDateTime = RightNow,
                EventType = entityState.ToString(),
                UserName = userName,
                TableName = entityEntry.Entity.GetType().Name
            });
            return returnValue;
        }
    }
}
