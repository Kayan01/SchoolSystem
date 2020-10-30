using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.LessonNote
{
    public class LessonNoteUploadVM
    {
        public string Name { get; set; }
        public long ClassId { get; set; }
        public long TeacherId { get; set; }
        public long SubjectId { get; set; }
        public DocumentVM FileObj { get; set; }
    }
}
