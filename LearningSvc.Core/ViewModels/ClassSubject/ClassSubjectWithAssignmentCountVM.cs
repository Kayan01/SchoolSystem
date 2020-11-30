using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.ClassSubject
{
    public class ClassSubjectWithAssignmentCountVM
    {
        public long Id { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public long AssignmentCount { get; set; }
    }
}
