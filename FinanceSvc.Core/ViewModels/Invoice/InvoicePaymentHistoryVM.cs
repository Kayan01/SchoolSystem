using FinanceSvc.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Invoice
{
    public class InvoicePaymentHistoryVM
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
        public string FeeGroup { get; set; }
        public string Session { get; set; }
        public DateTime DueDate { get; set; }


        public string TermName { get 
            {
                return Term.GetTermFromString(TermsJSON, TermSequence);
            } 
        }

        public int TermSequence { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public string TermsJSON { get; set; }

    }
}
