using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.LearningFile
{
    public class StudentFileVM
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string TeacherName { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileId { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public string Type { get; set; }
    }
}
