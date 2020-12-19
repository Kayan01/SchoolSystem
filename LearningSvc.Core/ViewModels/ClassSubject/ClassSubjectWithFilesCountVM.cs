using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.ClassSubject
{
    public class ClassSubjectWithFilesCountVM
    {
        public long Id { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public int FilesCount { get; set; }
    }
}
