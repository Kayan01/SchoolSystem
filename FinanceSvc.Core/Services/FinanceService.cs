using FinanceSvc.Core.Services.Interfaces;
using Shared.ViewModels;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services
{
    public class FinanceService : IFinanceService
    {

        public FinanceService()
        {
        }

        public async Task<ResultModel<string>> TestBroadcast(string title)
        {
            var result = new ResultModel<string>();
            result.Data = "Successful";
            return result;
        }
    }
}
