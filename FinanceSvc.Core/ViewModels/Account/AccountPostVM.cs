using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Account
{
    public class AccountPostVM
    {
        public long AccountTypeId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int AccountNumber { get; set; }
        public int OpeningBalance { get; set; }
        public int CashPostable { get; set; }

        public bool IsActive { get; set; }

    }
}
