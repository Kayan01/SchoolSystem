using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Subscription
{
    public class SubscriptionVM
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PricePerStudent { get; set; }
        public int ExpectedNumberOfStudent { get; set; }

        public string School { get; set; }
        public string SchoolGroup { get; set; }
    }
}
