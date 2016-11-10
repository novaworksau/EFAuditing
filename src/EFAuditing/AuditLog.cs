using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace EFAuditing
{
    [DoNotAudit]
    public class AuditLog
    {
        [Key]
        public long AuditLogId { get; set; }

        public Guid AuditBatchId { get; set; }

        public string UserName { get; set; }

        [Column("EventDateTime", TypeName = "datetime2(3)")]
        public DateTime EventDateTime { get; set; }

        public string EventType { get; set; }

        public string EntityId { get; set; }

        public string TableName { get; set; }

        public string Differences { get; set; }
    }
}
