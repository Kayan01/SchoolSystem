using AssessmentSvc.Core.ViewModels.SessionSetup;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Interfaces
{
    public interface ISessionSetup
    {
        Task<ResultModel<SessionSetupDetail>> AddSchoolSession(AddSessionSetupVM vM);
        Task<ResultModel<SessionSetupDetail>> UpdateSchoolSession(long Id, AddSessionSetupVM vM);
        Task<ResultModel<List<SessionSetupList>>> GetSchoolSessions();
        Task<ResultModel<SessionSetupDetail>> GetCurrentSchoolSession();
        Task<ResultModel<CurrentSessionAndTermVM>> GetCurrentSessionAndTerm();
    }
}
