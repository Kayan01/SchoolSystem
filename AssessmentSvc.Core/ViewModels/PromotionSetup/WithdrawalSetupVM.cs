using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.PromotionSetup
{
    public class WithdrawalSetupVM
    {
        public int MaxRepeat { get; set; }
        public List<WithdrawalReasonVM> WithdrawalReasons { get; set; }

        public static explicit operator WithdrawalSetupVM(Models.PromotionSetup model)
        {
            return model == null ? null : new WithdrawalSetupVM
            {
                MaxRepeat = model.MaxRepeat,
                WithdrawalReasons = model.WithdrawalReasons?.Select(m => new WithdrawalReasonVM()
                {
                    id = m.Id,
                    reason = m.Reason
                }).ToList()
            };
        }
    }

    public class WithdrawalReasonVM
    {
        public long id { get; set; }
        public string reason { get; set; }
    }
}
