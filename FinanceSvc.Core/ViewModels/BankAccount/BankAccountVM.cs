using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.BankAccount
{
    public class BankAccountVM
    {
        public long Id { get; set; }

        public string Bank { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }

        public bool IsActive { get; set; }
    }
}
