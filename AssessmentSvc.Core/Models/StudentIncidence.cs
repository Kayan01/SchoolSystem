using System;
using System.Collections.Generic;
using System.Text;
using Shared.Entities.Auditing;
using Shared.Tenancy;

namespace AssessmentSvc.Core.Models
{
    public class StudentIncidence : FullAuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        public DateTime OccurrenceDate { get; set; }

        public string Description { get; set; }

        public long StudentId { get; set; }
        public long SchoolClassId { get; set; }
        public long SessionId { get; set; }
        public int TermSequenceNumber { get; set; }
        public SchoolClass SchoolClass { get; set; }
        public Student Student { get; set; }
        public SessionSetup Session { get; set; }

    }
}
