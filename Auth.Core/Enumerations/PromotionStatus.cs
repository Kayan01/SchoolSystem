using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Auth.Core.Enumeration
{
    public enum PromotionStatus
    {
        Promoted,
        InPool,
        Graduated,
        Repeated, 
        Withdrawn, 
        [Display(Name = "Re-Instated")]
        Re_Instated
    }
}
