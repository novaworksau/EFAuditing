using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFAuditing
{
    public class PropertyLevelAuditLog : AuditLog
    {
        public Guid AuditBatchId { get; set; }

        public string KeyNames { get; set; }

        public string KeyValues { get; set; }

        public string ColumnName { get; set; }

        public string OriginalValue { get; set; }

        public string NewValue { get; set; }
    }
}
