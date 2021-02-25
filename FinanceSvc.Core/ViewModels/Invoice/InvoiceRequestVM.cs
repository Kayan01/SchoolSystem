using FinanceSvc.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Invoice
{
    public class InvoiceRequestVM
    {
        public string Session { get; set; }
        public string Term { get; set; }
        public long? ClassId { get; set; }
        public long? StudentId { get; set; }
        public InvoicePaymentStatus? PaymentStatus { get; set; }
    }
}
