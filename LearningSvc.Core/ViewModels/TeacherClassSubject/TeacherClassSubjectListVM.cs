using System;
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
        public long ClassSubjectId { get; set; }
        public long ClassId { get; set; }
        public long SubjectId { get; set; }
    }
}
