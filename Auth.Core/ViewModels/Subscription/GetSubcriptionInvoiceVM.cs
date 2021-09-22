using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Subscription
{
    public class GetSubcriptionInvoiceVM
    {
        public long InvoiceId { get; set; }
        public int NumberOfStudent { get; set; }
        public int AmountPerStudent { get; set; }
        public int AmountToBePaid
        {
            get
            {
                return NumberOfStudent * AmountPerStudent;
            }
        }
        public DateTime DueDate { get; set; }
        public long SchoolId { get; set; }
        public string SchoolName { get; set; }

        public bool Paid { get; set; }
        public DateTime PaidDate { get; set; }

        public string InvoiceType { get; set; }
    }
}
