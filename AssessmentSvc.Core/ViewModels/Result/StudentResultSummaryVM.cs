using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class StudentResultSummaryVM
    {
        public long StudentId { get; set; }
        public double Total { get; set; }
        public double Average { get; set; }
        public bool isApproved { get; set; }
    }
}
