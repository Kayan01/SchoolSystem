using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Invoice
{
    public class InvoicePostVM
    {
        public long ClassId { get; set; }
        public long FeeGroupId { get; set; }
        public string Session { get; set; }
        public string Term { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
