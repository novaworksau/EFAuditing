using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EFAuditing.TestHarness.TestModel
{
    public class CustomerInheritedFromBaseForAuditing : BaseCustomer
    {
        [Key]
        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Timestamp]
        public byte[] ConcurrencyStamp { get; set; }
    }
}
