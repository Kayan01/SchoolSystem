using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.Models
{
    public class InvoicePayment : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public long InvoiceId { get; set; }
        public decimal AmountPaid { get; set; }

        public Invoice Invoice { get; set; }
    }
}
