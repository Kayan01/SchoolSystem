using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class TimeTable : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

    }
}
