using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.Assignment
{
    public class AssignmentGetVM
    {
        public long Id { get; set; }
        public long ClassSubjectId { get; set; }
        public string SubjectName { get; set; }
        public string ClassName { get; set; }
        public string TeacherName { get; set; }

        public string Name { get; set; }
        public string OptionalComment { get; set; }
        public Guid FileId { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status {
            get
            {
                return DueDate >= DateTime.Now ? "Active" : "Due";
            }
        }
        public int NumberOfStudentsSubmitted { get; set; }
        public int TotalStudentsInClass { get; set; }
    }
}
