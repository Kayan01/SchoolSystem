using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models
{
    public class ResultSummary : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        public long StudentId { get; set; }
        public long SessionSetupId { get; set; }
        public long SchoolClassId { get; set; }
        public long SubjectId { get; set; }
        public int TermSequenceNumber { get; set; }
        public double ResultTotalAverage { get; set; }
        public bool ResultApproved { get; set; }


        public Subject Subject { get; set; }
        public SchoolClass SchoolClass { get; set; }
        public Student Student { get; set; }
        public SessionSetup Session { get; set; }
    }
}
