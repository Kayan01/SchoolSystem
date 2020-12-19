using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class IndividualBroadSheet
    {
        public  string ClassTeacherComment { get; set; }
        public string HeadTeacherComment { get; set; }
        public List<SubjectResultBreakdown> Breakdowns { get; set; } = new List<SubjectResultBreakdown>();
    }

    public class SubjectResultBreakdown
    {
        public string SubjectName { get; set; }
        public double CummulativeScore { get; set; }
        public string Grade { get; set; }
        public int Position { get; set; }
        public string Interpretation { get; set; }
        public List<AssesmentAndScoreViewModel> AssesmentAndScores { get; set; } = new List<AssesmentAndScoreViewModel>();
    }
}
