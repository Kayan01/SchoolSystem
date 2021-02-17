using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class IndividualBroadSheet
    {
        public List<SubjectResultBreakdown> Breakdowns { get; set; } = new List<SubjectResultBreakdown>();
    }
}
