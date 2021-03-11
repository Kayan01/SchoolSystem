using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.Models
{
    public class InvoiceComponent : AuditedEntity<long>
    {
        public long InvoiceId { get; set; }
        public string ComponentName { get; set; }
        public decimal Amount { get; set; }
        public bool IsCompulsory { get; set; }
        public bool IsSelected { get; set; }

        public Invoice Invoice { get; set; }
    }
}
