using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class ClassResultApprovalVM
    {
        public string ClassName { get; set; }
        public string DateCreated { get; set; }
        public bool isApproved { get; set; }
    }
}
