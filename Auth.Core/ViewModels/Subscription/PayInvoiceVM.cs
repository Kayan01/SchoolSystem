using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Subscription
{
    public class PayInvoiceVM
    {
        public long InvoiceId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
