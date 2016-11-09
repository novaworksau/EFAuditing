using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EFAuditing.TestHarness.TestModel
{
    public class CustomerNoAuditProperty
    {
        [Key]
        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        [DoNotAudit]
        public string LastName { get; set; }
    }
}
