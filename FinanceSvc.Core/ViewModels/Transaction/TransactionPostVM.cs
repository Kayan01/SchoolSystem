using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Transaction
{
    public class TransactionPostVM
    {
        public long InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
