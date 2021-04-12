using FinanceSvc.Core.Enumerations;
using FinanceSvc.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Invoice
{
    public class InvoiceVM
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
        public DateTime CreationDate { get; set; }
        public decimal Total { get; set; }
        public string FeeGroup { get; set; }
        public string Session { get; set; }
        public string TermName
        {
            get
            {
                return Term.GetTermFromString(TermsJSON, TermSequence);
            }
        }

        public int TermSequence { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public string TermsJSON { get; set; }

        public string Class { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public InvoiceApprovalStatus ApprovalStatus { get; set; }

        public string Status
        {
            get
            {
                return ApprovalStatus.ToString("F");
            }
        }
        public DateTime DueDate { get; set; }



        public decimal? InvoiceItemSubtotal
        {
            get
            {
                return InvoiceItems?.Sum(m => m.Amount);
            }
        }
        public List<InvoiceItemVM> InvoiceItems { get; set; }
    }
}
