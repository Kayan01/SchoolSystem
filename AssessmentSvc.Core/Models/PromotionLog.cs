using AssessmentSvc.Core.Enumeration;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models
{
    public class PromotionLog : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public PromotionStatus PromotionStatus { get; set; }
        public string Reason { get; set; }

        public long StudentId { get; set; }
        public long SessionSetupId { get; set; }
        public long SchoolClassId { get; set; }
        public int TermSequenceNumber { get; set; }

        public SchoolClass SchoolClass { get; set; }
        public Student Student { get; set; }
        public SessionSetup Session { get; set; }
    }
}
