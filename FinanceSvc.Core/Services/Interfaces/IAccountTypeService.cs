using FinanceSvc.Core.ViewModels.AccountType;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface IAccountTypeService
    {
        Task<ResultModel<List<AccountTypeVM>>> GetAccountTypes();
        Task<ResultModel<List<AccountTypeVM>>> GetAccountTypesByAccountClass(long accountClassId);
        Task<ResultModel<AccountTypeVM>> GetAccountType(long id);
        Task<ResultModel<string>> AddAccountType(AccountTypePostVM model);
        Task<ResultModel<string>> UpdateAccountType(long Id, AccountTypePostVM model);
    }
}
