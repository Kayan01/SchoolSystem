using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models
{
    public class Subject : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

    }
}
