using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models
{
    public class AssessmentSetup : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        public int SequenceNumber { get; set; }
        public string Name { get; set; }
        public int MaxScore { get; set; }
    }
}
