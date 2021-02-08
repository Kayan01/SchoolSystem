using FinanceSvc.Core.ViewModels.Component;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface IComponentService
    {
        Task<ResultModel<List<ComponentVM>>> GetComponents();
        Task<ResultModel<ComponentVM>> GetComponent(long id);
        Task<ResultModel<string>> AddComponent(ComponentPostVM model);
        Task<ResultModel<string>> UpdateComponent(long Id, ComponentPostVM model);
    }
}
