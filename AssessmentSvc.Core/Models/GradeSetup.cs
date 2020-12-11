using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models
{
    public class GradeSetup : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get ; set; }

        public string Grade { get; set; }
        public string Interpretation { get; set; }
        public double LowerBound { get; set; }
        public double UpperBound { get; set; }
        public bool IsActive { get; set; }
        public int Sequence { get; set; }
    }
}
