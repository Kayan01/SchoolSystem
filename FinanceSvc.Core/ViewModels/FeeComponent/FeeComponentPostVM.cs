using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.FeeComponent
{
    public class FeeComponentPostVM
    {
        public long ComponentId { get; set; }
        public int Amount { get; set; }
        public bool IsCompulsory { get; set; }
    }
}
