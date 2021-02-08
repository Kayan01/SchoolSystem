using FinanceSvc.Core.ViewModels;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface IAccountClassService
    {
        Task<ResultModel<List<AccountClassVM>>> GetAccountClasses();
        Task<ResultModel<AccountClassVM>> GetAccountClass(long id);
        Task<ResultModel<string>> AddAccountClass(AccountClassVM model);
        Task<ResultModel<string>> UpdateAccountClass(AccountClassVM model);
    }
}
