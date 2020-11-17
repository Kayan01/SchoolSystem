using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.LessonNote
{
    public class LessonNoteVM
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string ClassName { get; set; }
        public string TeacherName { get; set; }
        public string SubjectName { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }
        public byte[] File { get; set; }
    }
}
