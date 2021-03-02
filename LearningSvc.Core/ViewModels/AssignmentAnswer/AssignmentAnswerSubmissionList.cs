using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.AssignmentAnswer
{
    public class AssignmentAnswerSubmissionList
    {
        public long Id { get; set; }

        public string Title { get; set; }
        public string Status { get
            {
                return score == -1 ? "Submitted" : "Graded";
            } 
        }

        public string Score { get 
            {
                return score == -1 ? "" : $"{score}/{over}";
            } }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public double score { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public double over { get; set; }

        public DateTime SubmissionDate { get; set; }
    }
}
