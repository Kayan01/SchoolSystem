using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AssessmentSvc.Core.ViewModels;
using AssessmentSvc.Core.ViewModels.Result;
using Shared.ViewModels;

namespace AssessmentSvc.Core.Interfaces
{
    public interface IIncidenceService
    {
        Task<ResultModel<string>> InsertIncidenceReport(AddIncidenceVm model);
        Task<ResultModel<List<GetIncidenceVm>>> GetIncidenceReport(GetIncidenceQueryVm model);
    }
}
