﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.TeacherClassSubject
{
    public class TeacherClassSubjectListVM
    {
        public long Id { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public string Teacher { get; set; }
    }
}
