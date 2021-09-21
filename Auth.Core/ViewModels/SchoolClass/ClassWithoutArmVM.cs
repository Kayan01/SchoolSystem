using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.SchoolClass
{
    public class ClassWithoutArmVM
    {
        public string Name { get; set; }
        public int Sequence { get; set; }
        public bool IsTerminal { get; set; }
        public string Section { get; set; }
    }
}
