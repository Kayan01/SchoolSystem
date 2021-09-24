using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models
{
    public class SubscriptionInvoice : FullAuditedEntity<long>
    {
        public int NumberOfStudent { get; set; }
        public int AmountPerStudent { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public DateTime DueDate { get; set; }
        public bool Paid { get; set; }
        public DateTime PaidDate { get; set; }

        public long SchoolId { get; set; }
        public School School { get; set; }
    }
    public enum InvoiceType { Current, Arrears}
}
