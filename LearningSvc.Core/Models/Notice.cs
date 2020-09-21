using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class Notice : AuditedEntity<long>
    {
        public string Description { get; set; }
    }
}
