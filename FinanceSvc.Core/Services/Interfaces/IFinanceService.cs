using Shared.ViewModels;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface IFinanceService
    {
        Task<ResultModel<string>> TestBroadcast(string title);
    }
}
