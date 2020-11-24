using Microsoft.AspNetCore.Http;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.Media
{
    public class MediaUploadVM
    {
        public long ClassSubjectId { get; set; }
        public IFormFile FileObj { get; set; }
        public string Comment { get; set; }
    }
}
