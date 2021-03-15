using FinanceSvc.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Transaction
{
    public class TransactionDetailsVM : TransactionVM
    {
        public string PaymentDescription { get; set; }
        public string PaymentReference { get; set; }
        public PaymentChannel channel { get; set; }
        public string PaymentChannel
        {
            get
            {
                return channel.ToString("G");
            }
        }

    }
}
