﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// ReSharper disable once RedundantUsingDirective Since GetProperties Need this for dnxcore50
using System.Reflection;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;
using Microsoft.Data.Entity.Metadata;
using System.Runtime.InteropServices;

namespace EFAuditing
{
    /// <summary>
    /// The AuditLogBuilder class is used internally by AuditingDbContext.
    /// </summary>
    internal static class AuditLogBuilder
    {
        private const string KeySeperator = ";";
        [DllImport("rpcrt4.dll", SetLastError = true)]
        static extern int UuidCreateSequential(out Guid guid);

        internal static List<AuditLog> GetAuditLogsForExistingEntities(string userName,
            IEnumerable<EntityEntry> modifiedEntityEntries,
            IEnumerable<EntityEntry> deletedEntityEntries)
        {
            var auditLogs = new List<AuditLog>();
            foreach (
                var auditRecordsForEntityEntry in
                    modifiedEntityEntries.Select(
                        changedEntity => GetAuditLogs(changedEntity, userName, EntityState.Modified)))
                auditLogs.AddRange(auditRecordsForEntityEntry);
            foreach (
                var auditRecordsForEntityEntry in
                    deletedEntityEntries.Select(
                        changedEntity => GetAuditLogs(changedEntity, userName, EntityState.Deleted)))
                auditLogs.AddRange(auditRecordsForEntityEntry);
            return auditLogs;
        }

        internal static List<AuditLog> GetAuditLogsForAddedEntities(string userName,
            IEnumerable<EntityEntry> addedEntityEntries)
        {
            var auditLogs = new List<AuditLog>();
            foreach (
                var auditRecordsForEntityEntry in
                    addedEntityEntries.Select(
                        changedEntity => GetAuditLogs(changedEntity, userName, EntityState.Added)))
                auditLogs.AddRange(auditRecordsForEntityEntry);
            return auditLogs;
        }

        private static IEnumerable<AuditLog> GetAuditLogs(EntityEntry entityEntry, string userName, EntityState entityState)
        {
            var returnValue = new List<AuditLog>();
            var keyRepresentation = BuildKeyRepresentation(entityEntry, KeySeperator);
            DateTime RightNow = DateTime.Now;
            Guid AuditBatchID;
            UuidCreateSequential(out AuditBatchID);

            var auditedPropertyNames =
                entityEntry.Entity.GetType()
                    .GetProperties().Where(p => !p.GetCustomAttributes(typeof (DoNotAudit), true).Any())
                    .Select(info => info.Name);
            foreach (var propertyEntry in entityEntry.Metadata.GetProperties()
                .Where(x => auditedPropertyNames.Contains(x.Name))
                .Select(property => entityEntry.Property(property.Name)))
            {
                if(entityState == EntityState.Modified)
                    if (Convert.ToString(propertyEntry.OriginalValue) == Convert.ToString(propertyEntry.CurrentValue)) //Values are the same, don't log
                        continue;
                returnValue.Add(new AuditLog
                {
                    Differences = entityState == EntityState.Modified || entityState == EntityState.Added
                        ? Convert.ToString(propertyEntry.CurrentValue)
                        : null,
                    EventDateTime = RightNow,
                    EventType = entityState.ToString(),
                    UserName = userName,
                    TableName = entityEntry.Entity.GetType().Name
                });
            }
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
    }
}
