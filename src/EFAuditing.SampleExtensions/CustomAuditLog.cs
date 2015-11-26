using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace EFAuditing
{
    public class CustomAuditLog : AuditLog
    {
        public string Differences { get; set; }  
    }
}
