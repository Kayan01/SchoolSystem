using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AssessmentSvc.Core.Enumeration
{
    public enum PromotionStatus
    {
        Promoted, 
        Repeated, 
        Withdrawn, 
        [Display(Name = "Re-Instated")]
        Re_Instated
    }
}
