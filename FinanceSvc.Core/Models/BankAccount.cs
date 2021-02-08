using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.Models
{
    public class BankAccount : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        public string Bank { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }

        public bool IsActive { get; set; }
    }
}
