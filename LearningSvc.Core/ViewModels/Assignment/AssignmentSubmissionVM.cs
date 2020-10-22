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
        public string FileURL { get; set; }


        public static implicit operator AssignmentSubmissionVM(Models.AssignmentAnswer model)
        {
            return model == null ? null : new AssignmentSubmissionVM
            {
                Id = model.Id,
                StudentName = $"{model.Student?.FirstName} {model.Student?.LastName}",
                StudentNumber = model.Student?.UserId.ToString(),
                AssignmentTitle = model.Assignment?.Title,
                Comment = model.Comment,
                Score = model.Score,
                Date = model.DateSubmitted,
                FileURL = model.Attachment?.Path
            };
        }
    }
}
