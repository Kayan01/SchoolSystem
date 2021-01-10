using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class UpdateApprovedStudentResultViewModel
    {
        public string HeadTeacherComment { get; set; }
        public string ClassTeacherComment { get; set; }
        public long StudentId { get; set; }
        public long SessionId { get; set; }
        public long ClassId { get; set; }
        public int TermSequence { get; set; }
        public bool IsClassTeacherApproved { get; set; }
        public bool IsAdminApproved { get; set; }
    }
}
