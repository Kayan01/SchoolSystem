using FinanceSvc.Core.Enumerations;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FinanceSvc.Core.Models
{
    public class Invoice : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public long StudentId { get; set; }
        public long FeeId { get; set; }
        public long SessionSetupId { get; set; }
        public int TermSequenceNumber { get; set; }

        public DateTime PaymentDate { get; set; }
        public InvoicePaymentStatus PaymentStatus { get; set; }
        public InvoiceApprovalStatus ApprovalStatus { get; set; }

        public Student Student { get; set; }
        public Fee Fee { get; set; }
        public SessionSetup SessionSetup { get; set; }
        public List<InvoicePayment> Payments { get; set; }
    }
}
