using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.ClassSubject
{
    public class SubjectsToClassInsertVM
    {
        public long ClassId { get; set; }
        public long[] SubjectIds { get; set; }

    }
}
