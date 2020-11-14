using Microsoft.AspNetCore.Http;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.LessonNote
{
    public class LessonNoteUploadVM
    {
        public string Name { get; set; }
        public long ClassSubjectId { get; set; }
        public long TeacherId { get; set; }
        public IFormFile FileObj { get; set; }
        public string Comment { get; set; }
    }
}
