using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.FeeGroup
{
    public class FeeGroupPostVM
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
