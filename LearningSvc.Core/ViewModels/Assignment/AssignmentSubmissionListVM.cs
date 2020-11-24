using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.Assignment
{
    public class AssignmentSubmissionListVM
    {
        public long Id { get; set; }

        public string StudentName { get; set; }
        public string StudentNumber { get; set; }

        public string ClassName { get; set; }
        public DateTime Date { get; set; }
        
    }
}
