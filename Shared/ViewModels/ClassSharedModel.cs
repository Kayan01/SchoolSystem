using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class ClassSharedModel
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public string Name { get; set; }
        public string ClassArm { get; set; }
        public bool IsTerminalClass { get; set; }
        public int Sequence { get; set; }
    }
}
