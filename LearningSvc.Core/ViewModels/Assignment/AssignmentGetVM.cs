using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.Assignment
{
    public class AssignmentGetVM
    {
        public long Id { get; set; }

        public string SubjectName { get; set; }
        public string ClassName { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Active";
        public int? NumberOfStudentsSubmitted { get; set; }
        public int? TotalStudentsInClass { get; set; }


        public static implicit operator AssignmentGetVM(Models.Assignment model)
        {
            return model == null ? null : new AssignmentGetVM
            {
                Id = model.Id,
                SubjectName = model.Subject?.Name,
                ClassName = model.SchoolClass?.Name,
                CreationDate = model.CreationTime,
                DueDate = model.DueDate,
                Status = model.DueDate >= DateTime.Now ? "Active" : "Due",
                NumberOfStudentsSubmitted = model.AssignmentAnswers?.Count,
                TotalStudentsInClass = model.SchoolClass?.Students?.Count,
            };
        }
    }
}
