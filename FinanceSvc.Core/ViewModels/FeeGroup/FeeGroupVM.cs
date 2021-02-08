using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.FeeGroup
{
    public class FeeGroupVM
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }

    }
}
