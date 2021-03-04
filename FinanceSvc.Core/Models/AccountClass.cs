using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FinanceSvc.Core.Models
{
    public class AccountClass : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        public string Name { get; set; }
        public int MinNumberValue { get; set; }
        public int MaxNumberValue { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public ICollection<AccountType> AccountTypes { get; set; } = new List<AccountType>();

    }
}
