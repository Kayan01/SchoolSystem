using FinanceSvc.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Invoice
{
    public class InvoiceDetailVM
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
        public string StudentName { get; set; }
        public string Class { get; set; }
        public string Session { get; set; }
        public string Term { get; set; }
        public decimal Total { get; set; }
        public string FeeGroup { get; set; }
        public bool ComponentSelected { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public InvoiceApprovalStatus approvalStatus { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public InvoicePaymentStatus paymentStatus { get; set; }

        public string ApprovalStatus
        {
            get
            {
                return approvalStatus.ToString("F");
            }
        }
        public string PaymentStatus
        {
            get
            {
                if (paymentStatus == InvoicePaymentStatus.Unpaid)
                {
                    var now = DateTime.Now;
                    if (DueDate > now)
                    {
                        return "Outstanding";
                    }
                    return "Overdue";
                }

                return paymentStatus.ToString("F");
            }
        }

        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }

        public List<InvoiceItemVM> InvoiceItems { get; set; }
    }
}
