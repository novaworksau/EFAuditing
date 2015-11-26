using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFAuditing.TestHarness.Helpers
{
    public class TestableDbAuditStoreProvider : IExternalAuditStoreProvider
    {
        public List<AuditLog> _auditLogs = new List<AuditLog>();

        public IEnumerable<AuditLog> ReadAuditLogs()
        {
            return _auditLogs;
        }

        public async Task WriteAuditLogs(IEnumerable<AuditLog> auditLogs)
        {
            await Task.Factory.StartNew(() => this._auditLogs.AddRange(auditLogs));
        }
    }
}
