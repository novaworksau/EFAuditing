using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace EFAuditing
{
    public class AuditLog
    {
        [Key]
        public long AuditLogId { get; set; }
               
        [Column("EventDateTime", TypeName = "datetime2(3)")]
        public DateTime EventDateTime { get; set; }

        public string EventType { get; set; }

        public string UserName { get; set; }

        public string EntityName { get; set; }

        public string Differences { get; set; }  
    }
}
