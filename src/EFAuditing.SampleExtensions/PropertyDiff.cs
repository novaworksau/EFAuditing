using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFAuditing.SampleExtensions
{
    public class PropertyDiff
    {
        public string PropertyName { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
    }
}
