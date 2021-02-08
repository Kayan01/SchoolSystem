using FinanceSvc.Core.ViewModels.FeeComponent;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface IFeeComponentService
    {
        Task<ResultModel<string>> UpdateFeeComponent(long Id, FeeComponentPostVM model);
    }
}
