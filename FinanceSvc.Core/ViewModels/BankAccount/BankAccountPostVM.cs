using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels.BankAccount
{
    public class BankAccountPostVM
    {
        public string Bank { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }

        public bool IsActive { get; set; }
    }
}
