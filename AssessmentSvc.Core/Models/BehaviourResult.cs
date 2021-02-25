using System;
using System.Collections.Generic;
using System.Text;
using Shared.Entities.Auditing;
using Shared.Tenancy;

namespace AssessmentSvc.Core.Models
{
    public class BehaviourResult: FullAuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        public string Type { get; set; }
        public string Name { get; set; }

        public string Grade { get; set; }

        public long StudentId { get; set; }
        public long SchoolClassId { get; set; }
        public long SessionId { get; set; }
        public int TermSequenceNumber { get; set; }
        public SchoolClass SchoolClass { get; set; }
        public Student Student { get; set; }
        public SessionSetup Session { get; set; }
    }
}
