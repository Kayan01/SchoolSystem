using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.ClassSubject
{
    public class ClassSubjectsInsertVM
    {
        public long ClassId { get; set; }
        public long[] SubjectIds { get; set; }

    }
}
