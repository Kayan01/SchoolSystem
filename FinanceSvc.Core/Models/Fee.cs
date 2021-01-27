using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.Models
{
    public class Fee : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public long SchoolClassId { get; set; }
        public long FeeGroupId { get; set; }

        public string Name { get; set; }
        public string Terms { get; set; }
        public int SequenceNumber { get; set; }

        public bool IsActive { get; set; }

        public SchoolClass SchoolClass { get; set; }
        public FeeGroup FeeGroup { get; set; }
        public ICollection<FeeComponent> FeeComponents { get; set; } = new List<FeeComponent>();
    }
}
