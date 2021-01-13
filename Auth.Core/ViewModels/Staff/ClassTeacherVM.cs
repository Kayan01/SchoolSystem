using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Staff
{
    public class ClassTeacherVM
    {
        public long TeacherId { get; set; }
        public long ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassSection { get; set; }
    }
}
