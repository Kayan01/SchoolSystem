using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Subscription
{
    public class AddSubscriptionVM
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PricePerStudent { get; set; }
        public int ExpectedNumberOfStudent { get; set; }

        public long SchoolId { get; set; }
    }
}
