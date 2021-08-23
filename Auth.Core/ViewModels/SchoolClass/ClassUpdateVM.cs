using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.SchoolClass
{
    public class ClassUpdateVM
    {
        public long Id { get; set; }
        public string Name { get;  set; }
        public int Sequence { get;  set; }
        public bool IsTerminalClass { get;  set; }
    }
}
