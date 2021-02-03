using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.AccountType
{
    public class AccountTypePostVM
    {
        public long AccountClassId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
