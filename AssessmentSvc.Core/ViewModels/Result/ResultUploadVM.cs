using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Result
{
    public class ResultUploadVM
    {
        public long SubjectId { get; set; }
        public long ClassId { get; set; }
        public List<StudentResult> StudentResults { get; set; }
    }

    public class StudentResult 
    {
        public long StudentId { get; set; }
        public List<AssessmentAndScore> AssessmentAndScores { get; set; }
    }

    public class AssessmentAndScore
    {
        public long AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public int Score { get; set; }
    }
}
