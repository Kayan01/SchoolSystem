using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Interfaces
{
    public interface IPromotionService
    {
        Task<ResultModel<string>> PromoteAllStudent();
    }
}
