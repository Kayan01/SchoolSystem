using AssessmentSvc.Core.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class UpdateApprovedStudentResultViewModel
    {
        [Required]
        public long HeadTeacherId { get; set; }
        [Required]
        public long ClassTeacherId { get; set; }
        public string HeadTeacherComment { get; set; }
        public string ClassTeacherComment { get; set; }
        public long StudentId { get; set; }
        public long SessionId { get; set; }
        public long ClassId { get; set; }
        public int TermSequence { get; set; }
        public ApprovalStatus? ClassTeacherApprovalStatus { get; set; }
        public ApprovalStatus? AdminApprovalStatus { get; set; }
        public ApprovalStatus? HeadTeacherApprovalStatus { get; set; }
    }
}
