using FinanceSvc.Core.ViewModels.Fee;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface IFeeService
    {
        Task<ResultModel<List<FeeVM>>> GetFees();
        Task<ResultModel<FeeWithComponentVM>> GetFee(long id);
        Task<ResultModel<string>> AddFee(FeePostVM model);
        Task<ResultModel<string>> UpdateFee(long Id, FeePostVM model);
    }
}
