using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.Models
{
    public class AccountType : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        public string Name { get; set; }
        public int MinNumberValue { get; set; }
        public int MaxNumberValue { get; set; }

        public bool IsActive { get; set; }

        public ICollection<AccountClass> AccountClasses { get; set; } = new List<AccountClass>();

    }
}
