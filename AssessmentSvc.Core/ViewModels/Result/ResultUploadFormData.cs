using AssessmentSvc.Core.ViewModels.AssessmentSetup;
using AssessmentSvc.Core.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class ResultUploadFormData
    {
        public List<AssessmentSetupVM> Assessments { get; set; }
        public List<StudentVM> Students { get; set; }
    }
}
