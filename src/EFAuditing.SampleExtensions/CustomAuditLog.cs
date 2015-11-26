using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace EFAuditing
{
    public class CustomAuditLog : AuditLog
    {
        
        public new List<Tuple<string, string, string>> Differences { get; set; }  
    }
}
