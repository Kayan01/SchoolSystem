using AssessmentSvc.Core.ViewModels.PromotionSetup;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Interfaces
{
    public interface IPromotionSetupService
    {
        Task<ResultModel<PromotionSetupVM>> GetPromotionSetup();
        Task<ResultModel<WithdrawalSetupVM>> GetWithdrawalSetup();
        Task<ResultModel<PromotionSetupVM>> AddOrUpdatePromotionSetup(PromotionSetupVM vm);
        Task<ResultModel<WithdrawalSetupVM>> AddOrUpdateWithdrawalSetup(WithdrawalSetupVM vm);
    }
}
