using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFAuditing
{
    public interface IAuditLogBuilder
    {
        List<AuditLog> GetAuditLogsForAddedEntities(string userName, IEnumerable<EntityEntry> addedEntityEntries);

        List<AuditLog> GetAuditLogsForDeletedEntities(string userName, IEnumerable<EntityEntry> deletedEntityEntries);

        List<AuditLog> GetAuditLogsForExistingEntities(string userName, IEnumerable<EntityEntry> modifiedEntityEntries);
    }
}