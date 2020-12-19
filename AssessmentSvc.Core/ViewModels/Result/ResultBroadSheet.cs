using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class ResultBroadSheet
    {
        public string StudentName { get; set; }
        public string StudentRegNo { get; set; }
        public List<SubjectResultBroadSheet> AssessmentAndScores { get; set; } = new List<SubjectResultBroadSheet>();
        public double? AverageScore { get {
                return AssessmentAndScores == null ? 0 : 
                    Math.Round(AssessmentAndScores.Select(x => x.Score).Sum() / AssessmentAndScores.Count, 1, MidpointRounding.AwayFromZero);
            } }
    }

    public class SubjectResultBroadSheet
    {
        public string SubjectName { get; set; }
        public double Score { get; set; }
    }
}
