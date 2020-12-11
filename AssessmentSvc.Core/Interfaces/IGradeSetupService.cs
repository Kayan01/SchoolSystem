using AssessmentSvc.Core.ViewModels.GradeSetup;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Interfaces
{
    public interface IGradeSetupService
    {
        Task<ResultModel<List<GradeSetupListVM>>> GetAllGradeForSchoolSetup();
        Task<ResultModel<GradeSetupVM>> GetGradeSetupById(long Id);
        Task<ResultModel<GradeSetupVM>> UpdateGradeSetup(long id,GradeSetupVM model);
        Task<ResultModel<List<GradeSetupListVM>>> AddGradeSetup(List<GradeSetupVM> models);
    }
}
