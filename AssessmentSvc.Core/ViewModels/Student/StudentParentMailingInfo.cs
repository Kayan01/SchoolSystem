using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Student
{
    public class StudentParentMailingInfo
    {
        public long StudentId { get; set; }
        public string Email { get; set; }
        public string ParentName { get; set; }
        public string StudentName { get; set; }
    }
}
