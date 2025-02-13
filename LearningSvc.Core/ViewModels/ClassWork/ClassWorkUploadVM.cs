﻿using Microsoft.AspNetCore.Http;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.ClassWork
{
    public class ClassWorkUploadVM
    {
        public string Comment { get; set; }
        public long ClassSubjectId { get; set; }
        public IFormFile FileObj { get; set; }
    }
}
