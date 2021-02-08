using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.Models
{
    public class FeeComponent : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public long FeeId { get; set; }
        public long ComponentId { get; set; }

        public int Amount { get; set; }

        public bool IsCompulsory { get; set; }

        public Fee Fee { get; set; }
        public Component Component { get; set; }
    }
}
