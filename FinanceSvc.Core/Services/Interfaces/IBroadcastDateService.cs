using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface IBroadcastDateService
    {
        Task<ResultModel<string>> GetDataFromAuth(DateTime startDate, DateTime endDate);
    }
}
