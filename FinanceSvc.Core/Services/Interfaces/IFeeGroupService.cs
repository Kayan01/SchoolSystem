using FinanceSvc.Core.ViewModels.FeeGroup;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface IFeeGroupService
    {
        Task<ResultModel<List<FeeGroupVM>>> GetFeeGroups();
        Task<ResultModel<FeeGroupVM>> GetFeeGroup(long id);
        Task<ResultModel<string>> AddFeeGroup(FeeGroupPostVM model);
        Task<ResultModel<string>> UpdateFeeGroup(long Id, FeeGroupPostVM model);
    }
}
