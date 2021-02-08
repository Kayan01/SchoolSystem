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

        public long AccountClassId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public AccountClass AccountClass { get; set; }
    }
}
