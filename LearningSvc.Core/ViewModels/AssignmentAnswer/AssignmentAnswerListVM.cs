using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.AssignmentAnswer
{
    public class AssignmentAnswerListVM
    {
        public long Id { get; set; }

        public string StudentName { get; set; }
        public string StudentNumber { get; set; }

        public string ClassName { get; set; }
        public DateTime Date { get; set; }
        public double Score { get; set; }
        public string FileType { get; set; }
        public string File { get; set; }
    }
}
