using System.Collections.Generic;
using Microsoft.Data.Entity.ChangeTracking;

namespace EFAuditing
{
    public interface IAuditLogBuilder
    {
        List<AuditLog> GetAuditLogsForAddedEntities(string userName, IEnumerable<EntityEntry> addedEntityEntries);
        List<AuditLog> GetAuditLogsForExistingEntities(string userName, IEnumerable<EntityEntry> modifiedEntityEntries, IEnumerable<EntityEntry> deletedEntityEntries);
    }
}