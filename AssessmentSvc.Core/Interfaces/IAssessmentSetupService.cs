using AssessmentSvc.Core.ViewModels.AssessmentSetup;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Interfaces
{
    public interface IAssessmentSetupService
    {
        Task<ResultModel<List<AssessmentSetupVM>>> GetAllAssessmentSetup();
        Task<ResultModel<AssessmentSetupVM>> GetAssessmentSetup(long Id);
        Task<ResultModel<string>> UpdateAssessmentSetup(List<AssessmentSetupVM> model);
        Task<ResultModel<List<AssessmentSetupVM>>> AddAssessmentSetup(List<AssessmentSetupUploadVM> models);
        Task<ResultModel<string>> RemoveAssessmentSetup(long Id);
    }
}
