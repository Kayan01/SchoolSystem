using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.Models
{
    public class Discount : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string Name { get; set; }
        public int Percentage { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Component> Components { get; set; } = new List<Component>();
    }
}
