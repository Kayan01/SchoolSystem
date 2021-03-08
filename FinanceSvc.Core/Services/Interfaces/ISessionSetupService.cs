using FinanceSvc.Core.ViewModels.SessionSetup;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface ISessionSetupService
    {
        void AddOrUpdateSessionFromBroadcast(SessionSharedModel model);
        Task<ResultModel<CurrentSessionAndTermVM>> GetSessionAndTerm(long sessionId, int termSequenceNumber);
        Task<ResultModel<CurrentSessionAndTermVM>> GetCurrentSessionAndTerm();
    }
}
