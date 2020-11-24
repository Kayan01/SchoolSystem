using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.TeacherClassSubject
{
    public class TeacherClassSubjectInsertVM
    {
        public long TeacherId { get; set; }
        public long[] ClassSubjectIds { get; set; }
    }
}
