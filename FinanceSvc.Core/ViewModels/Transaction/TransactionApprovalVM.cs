using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
}
