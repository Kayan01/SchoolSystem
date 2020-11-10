using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class Student : Person, ITenantModelType
    {
        public long TenantId { get; set; }
        public long? ClassId { get; set; }


        public SchoolClass Class { get; set; }
        public ICollection<AssignmentAnswer> AssignmentAnswers { get; set; }
    }
}
