using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models
{
    public class Student : Person, ITenantModelType
    {
        public long TenantId { get; set; }
        public long? ClassId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; }
        public string ParentName { get; set; }
        public string ParentEmail { get; set; }

        public SchoolClass Class { get; set; }
    }
}
