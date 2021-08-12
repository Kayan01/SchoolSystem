using AssessmentSvc.Core.Enumeration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.PromotionSetup
{
    public class PromotionSetupVM
    {
        public PromotionMethod PromotionMethod { get; set; }
        public PromotionType PromotionType { get; set; }
        public int PromotionScore { get; set; }


        public static explicit operator PromotionSetupVM(Models.PromotionSetup model)
        {
            return model == null ? null : new PromotionSetupVM
            {
                PromotionMethod = model.PromotionMethod,
                PromotionType = model.PromotionType,
                PromotionScore = model.PromotionScore,
            };
        }
    }
}
