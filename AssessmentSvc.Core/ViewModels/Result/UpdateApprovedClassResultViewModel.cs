using AssessmentSvc.Core.Enumeration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class UpdateApprovedClassResultViewModel
    {
        public string HeadTeacherComment { get; set; }
        public string ClassTeacherComment { get; set; }
        public long ClassId { get; set; }
        public ApprovalStatus? ClassTeacherApprovalStatus { get; set; }
        public ApprovalStatus? AdminApprovalStatus { get; set; }
        public ApprovalStatus? HeadTeacherApprovalStatus { get; set; }
    }
}
