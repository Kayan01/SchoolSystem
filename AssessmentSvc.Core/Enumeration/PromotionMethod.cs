using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AssessmentSvc.Core.Enumeration
{
    public enum PromotionMethod
    {
        [Display(Name = "Based on Terms Completed")]
        Based_On_Terms_Completed, 
        [Display(Name = "Based on School Session")]
        Based_On_School_Session
    }
}
