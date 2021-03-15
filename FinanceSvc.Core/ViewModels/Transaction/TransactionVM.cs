using FinanceSvc.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Transaction
{
    public class TransactionVM
    {
        public long TransactionId { get; set; }
        public string TransactionNumber
        {
            get
            {
                return $"TRN-{TransactionId.ToString("0000000")}";
            }
        }
        public long InvoiceId { get; set; }
        public string InvoiceNumber
        {
            get
            {
                return $"INV-{InvoiceId.ToString("0000000")}";
            }
        }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string FeeType { get; set; }
        public string FileId { get; set; }
        public DateTime DueDate { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public TransactionStatus status { get; set; }
        public string Status { get 
            {
                return status.ToString("G");
            } }

        public string StudentRegNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal Outstanding { get 
            {
                return TotalAmount - TotalPaid;
            } }
    }
}
