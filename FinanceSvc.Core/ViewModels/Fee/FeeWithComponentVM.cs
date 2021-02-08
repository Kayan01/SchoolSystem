using FinanceSvc.Core.ViewModels.FeeComponent;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Fee
{
    public class FeeWithComponentVM : FeeVM
    {
        public ICollection<FeeComponentVM> FeeComponents { get; set; }
    }
}
