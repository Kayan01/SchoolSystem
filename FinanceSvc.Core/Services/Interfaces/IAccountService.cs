using FinanceSvc.Core.ViewModels.Account;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResultModel<List<AccountVM>>> GetAccounts();
        Task<ResultModel<List<AccountVM>>> GetAccountsByAccountClass(long accountClassId);
        Task<ResultModel<AccountVM>> GetAccount(long id);
        Task<ResultModel<string>> AddAccount(AccountPostVM model);
        Task<ResultModel<string>> UpdateAccount(long Id, AccountPostVM model);
    }
}
