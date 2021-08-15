using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class StudentResultSummaryVM
    {
        public string StudentName { get; set; }
        public string StudentRegNumber { get; set; }
        public double Average { get; set; }
        public bool isApproved { get; set; }
    }
}
