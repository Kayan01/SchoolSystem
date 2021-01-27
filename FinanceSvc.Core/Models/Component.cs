using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.Models
{
    public class Component : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public long AccountId { get; set; }
        public long? DiscountId { get; set; }

        public string Name { get; set; }
        public string Terms { get; set; }
        public int SequenceNumber { get; set; }

        public bool IsActive { get; set; }

        public Account Account { get; set; }
        public Discount Discount { get; set; }
        public ICollection<FeeComponent> FeeComponents { get; set; } = new List<FeeComponent>();
    }
}
