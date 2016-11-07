using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// ReSharper disable once RedundantUsingDirective Since GetProperties Need this for dnxcore50
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace EFAuditing
{
    /// <summary>
    /// The AuditLogBuilder class is used internally by AuditingDbContext.
    /// </summary>
    public class AuditLogBuilder : IAuditLogBuilder
    {
        private const string KeySeperator = ";";

        private static IEnumerable<AuditLog> GetAuditLogs(EntityEntry entityEntry, string userName, EntityState entityState)
        {
            var returnValue = new List<PropertyLevelAuditLog>();
            var keyRepresentation = BuildKeyRepresentation(entityEntry, KeySeperator);
            DateTime RightNow = DateTime.Now;
            //Guid gets created for each change set.
            Guid AuditBatchID = Guid.NewGuid();

            //this is the list that will be serialized
            var differences = new List<PropertyDiff>();


            var auditedPropertyNames =
                entityEntry.Entity.GetType()
                    .GetProperties().Where(p => !p.GetCustomAttributes(typeof(DoNotAudit), true).Any())
                    .Select(info => info.Name);

            foreach (var propertyEntry in entityEntry.Metadata.GetProperties()
                .Where(x => auditedPropertyNames.Contains(x.Name))
                .Select(property => entityEntry.Property(property.Name)))
            {
                if (entityState == EntityState.Modified)
                    if (Convert.ToString(propertyEntry.OriginalValue) == Convert.ToString(propertyEntry.CurrentValue)) //Values are the same, don't log
                        continue;

                var diff = new PropertyDiff
                {
                    PropertyName = propertyEntry.GetType().Name,
                    previousValue = entityState != EntityState.Added ? Convert.ToString(propertyEntry.OriginalValue) : null,
                    newValue = entityState == EntityState.Modified || entityState == EntityState.Added ? Convert.ToString(propertyEntry.CurrentValue) : null
                };

                var serializedData = JsonConvert.SerializeObject(diff);

                returnValue.Add(new PropertyLevelAuditLog
                {
                    AuditBatchId = AuditBatchID,
                    KeyNames = keyRepresentation.Key,
                    EntityId = keyRepresentation.Value,
                    OriginalValue = entityState != EntityState.Added
                        ? Convert.ToString(propertyEntry.OriginalValue)
                        : null,
                    NewValue = entityState == EntityState.Modified || entityState == EntityState.Added
                        ? Convert.ToString(propertyEntry.CurrentValue)
                        : null,
                    ColumnName = propertyEntry.Metadata.Name,
                    EventDateTime = RightNow,
                    EventType = entityState.ToString(),
                    UserName = userName,
                    Differences = serializedData,
                    TableName = entityEntry.Entity.GetType().Name
                });
            }
            return returnValue;
        }

        private AuditLog GetAddedAuditLogs(EntityEntry entityEntry, string userName, EntityState entityState)
        {
            var keyRepresentation = BuildKeyRepresentation(entityEntry, KeySeperator);
            DateTime RightNow = DateTime.Now;
            //Guid gets created for each change set.
            Guid AuditBatchID = Guid.NewGuid();

            //this is the list that will be serialized
            var differences = new List<PropertyDiff>();


            var auditedPropertyNames =
                entityEntry.Entity.GetType()
                    .GetProperties().Where(p => !p.GetCustomAttributes(typeof(DoNotAudit), true).Any())
                    .Select(info => info.Name);

            foreach (var propertyEntry in entityEntry.Metadata.GetProperties()
                .Where(x => auditedPropertyNames.Contains(x.Name))
                .Select(property => entityEntry.Property(property.Name)))
            {

                var diff = new PropertyDiff
                {
                    PropertyName = propertyEntry.Metadata.Name,
                    newValue = entityState == EntityState.Added ? Convert.ToString(propertyEntry.CurrentValue) : null
                };
                differences.Add(diff);
            }

            var serializedData = JsonConvert.SerializeObject(differences);

            var returnValue = new AuditLog()
            {
                AuditBatchId = AuditBatchID,
                EntityId = keyRepresentation.Value,
                EventDateTime = RightNow,
                EventType = entityState.ToString(),
                UserName = userName,
                Differences = serializedData,
                TableName = entityEntry.Entity.GetType().Name
            };
            return returnValue;
        }


        private AuditLog GetDeletedAuditLogs(EntityEntry entityEntry, string userName, EntityState entityState)
        {
            var keyRepresentation = BuildKeyRepresentation(entityEntry, KeySeperator);
            DateTime RightNow = DateTime.Now;
            //Guid gets created for each change set.
            Guid AuditBatchID = Guid.NewGuid();

            //this is the list that will be serialized
            var differences = new List<PropertyDiff>();


            var auditedPropertyNames =
                entityEntry.Entity.GetType()
                    .GetProperties().Where(p => !p.GetCustomAttributes(typeof(DoNotAudit), true).Any())
                    .Select(info => info.Name);

            foreach (var propertyEntry in entityEntry.Metadata.GetProperties()
                .Where(x => auditedPropertyNames.Contains(x.Name))
                .Select(property => entityEntry.Property(property.Name)))
            {

                var diff = new PropertyDiff
                {
                    PropertyName = propertyEntry.Metadata.Name,
                    previousValue = entityState == EntityState.Deleted ? Convert.ToString(propertyEntry.OriginalValue) : null,
                    newValue = "N/A"
                };
                differences.Add(diff);
            }

            var serializedData = JsonConvert.SerializeObject(differences);

            var returnValue = new AuditLog()
            {
                AuditBatchId = AuditBatchID,
                EntityId = keyRepresentation.Value,
                EventDateTime = RightNow,
                EventType = entityState.ToString(),
                UserName = userName,
                Differences = serializedData,
                TableName = entityEntry.Entity.GetType().Name
            };
            return returnValue;
        }

        private static KeyValuePair<string, string> BuildKeyRepresentation(EntityEntry entityEntry, string seperator)
        {
            var keyProperties = entityEntry.Metadata.GetProperties().Where(x => x.IsPrimaryKey()).ToList();
            if (keyProperties == null)
                throw new ArgumentException("No key found in");
            var keyPropertyEntries =
                keyProperties.Select(keyProperty => entityEntry.Property(keyProperty.Name)).ToList();
            var keyNameString = new StringBuilder();
            foreach (var keyProperty in keyProperties)
            {
                keyNameString.Append(keyProperty.Name);
                keyNameString.Append(seperator);
            }
            keyNameString.Remove(keyNameString.Length - 1, 1);
            var keyValueString = new StringBuilder();
            foreach (var keyPropertyEntry in keyPropertyEntries)
            {
                keyValueString.Append(keyPropertyEntry.CurrentValue);
                keyValueString.Append(seperator);
            }
            keyValueString.Remove(keyValueString.Length - 1, 1);
            var key = keyNameString.ToString();
            var value = keyValueString.ToString();
            return new KeyValuePair<string, string>(key, value);
        }

        public List<AuditLog> GetAuditLogsForAddedEntities(string userName, IEnumerable<EntityEntry> addedEntityEntries)
        {
            var auditLogs = new List<AuditLog>();
            foreach (
                var auditRecordsForEntityEntry in
                    addedEntityEntries.Select(
                        changedEntity => GetAddedAuditLogs(changedEntity, userName, EntityState.Added)))
                auditLogs.Add(auditRecordsForEntityEntry);
            return auditLogs;
        }

        public List<AuditLog> GetAuditLogsForDeletedEntities(string userName, IEnumerable<EntityEntry> deletedEntityEntries)
        {
            var auditLogs = new List<AuditLog>();
            foreach (
           var auditRecordsForEntityEntry in
               deletedEntityEntries.Select(
                   changedEntity => GetDeletedAuditLogs(changedEntity, userName, EntityState.Deleted)))
                auditLogs.Add(auditRecordsForEntityEntry);

            return auditLogs;
        }



        public List<AuditLog> GetAuditLogsForExistingEntities(string userName, IEnumerable<EntityEntry> modifiedEntityEntries)
        {
            var auditLogs = new List<AuditLog>();
            foreach (
                var auditRecordsForEntityEntry in
                    modifiedEntityEntries.Select(
                        changedEntity => GetAuditLogs(changedEntity, userName, EntityState.Modified)))
                auditLogs.AddRange(auditRecordsForEntityEntry);

            return auditLogs;
        }
    }
}
