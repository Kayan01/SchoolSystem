using LearningSvc.Core.ViewModels.SchoolClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.Subject
{
    public class SubjectUpdateVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
