using FinanceSvc.Core.Enumerations;
using Shared.Entities;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.Models
{
    public class Transaction : FullAuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public long InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public TransactionStatus Status { get; set; }
        public Guid? FileUploadId { get; set; }

        public FileUpload FileUpload { get; set; }
        public Invoice Invoice { get; set; }
    }
}
