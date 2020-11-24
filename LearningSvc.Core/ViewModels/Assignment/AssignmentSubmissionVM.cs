using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearningSvc.Core.ViewModels.Assignment
{
    public class AssignmentSubmissionVM
    {
        public long Id { get; set; }

        public string AssignmentTitle { get; set; }
        public string StudentName { get; set; }
        public string StudentNumber { get; set; }

        public double Score { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public Guid FileId { get; set; }

    }
}
