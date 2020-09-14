using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class Attendance : AuditedEntity<long>
    {
        public long ClassSessionId { get; set; }
        public long StudentId { get; set; }
    }
}
