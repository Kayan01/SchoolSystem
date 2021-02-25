using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.FeeComponent
{
    public class FeeComponentVM
    {
        public long Id { get; set; }
        public long ComponentId { get; set; }
        public long FeeId { get; set; }
        public string Fee { get; set; }
        public string Component { get; set; }

        public decimal Amount { get; set; }

        public bool IsCompulsory { get; set; }

    }
}
