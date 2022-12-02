using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Transactions;

namespace FinanceSvc.Core.ViewModels.Transaction
{
    public class TransactionApprovalVM
    {
        [Required]
        public long TransactionId { get; set; }
        [Required]
        public bool Approve { get; set; }
        public string Comment { get; set; }
    }

    public class TransStatus
    {
        public Enumerations.TransactionStatus Status { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class ExportPayloadVM
    {
        public string FileName { get; set; }
        public string Base64String { get; set; }
    }
}
