﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.Subject
{
    public class SubjectInsertVM
    {
        public string Name { get; set; }
        public long[] ClassIds { get; set; }
        public bool IsActive { get; set; }
    }
}
