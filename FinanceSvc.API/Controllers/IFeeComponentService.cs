using FinanceSvc.Core.ViewModels.FeeComponent;
using Shared.ViewModels;
using System.Threading.Tasks;

namespace FinanceSvc.API.Controllers
{
    public interface IFeeComponentService
    {
        Task<ResultModel<string>> UpdateFeeComponent(long Id, FeeComponentPostVM model);
    }
}