using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FinanceSvc.Core.Models
{
    public class AccountType : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        [NotMapped]
        public long AccountClassId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public AccountClass AccountClass { get; set; }
    }
}
