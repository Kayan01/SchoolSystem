using FinanceSvc.Core.ViewModels.BankAccount;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface IBankAccountService
    {
        Task<ResultModel<List<BankAccountVM>>> GetBankAccounts();
        Task<ResultModel<BankAccountVM>> GetBankAccount(long id);
        Task<ResultModel<string>> AddBankAccount(BankAccountPostVM model);
        Task<ResultModel<string>> UpdateBankAccount(long Id, BankAccountPostVM model);
    }
}
