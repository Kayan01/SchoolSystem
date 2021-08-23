using AssessmentSvc.Core.Enumeration;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models
{
    public class SchoolPromotionLog : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public long SessionSetupId { get; set; }
        public string Payload { get; set; }

        public SessionSetup Session { get; set; }
    }
}
