using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.LessonNote
{
    public class LessonNoteListVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string TeacherName { get; set; }
        public string SubjectName { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid FileId { get; set; }

    }
}
