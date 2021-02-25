using FinanceSvc.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Invoice
{
    public class InvoicePaymentVM
    {
        public long InvoiceId { get; set; }
        public string InvoiceNumber
        {
            get
            {
                return $"INV-{InvoiceId.ToString("0000000")}";
            }
        }
        public string StudentRegNumber { get; set; }
        public decimal Total { get; set; }
        public decimal Paid { get; set; }
        public decimal Outstanding
        {
            get
            {
                return Total - Paid;
            }
        }
        public string FeeGroup { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public InvoicePaymentStatus PaymentStatus { get; set; }

        public string Status
        {
            get
            {
                if (PaymentStatus == InvoicePaymentStatus.Unpaid)
                {
                    var now = DateTime.Now;
                    if (DueDate > now)
                    {
                        return "Outstanding";
                    }
                    return "Overdue";
                }

                return PaymentStatus.ToString("F");
            }
        }
        public DateTime DueDate { get; set; }
    }
}
