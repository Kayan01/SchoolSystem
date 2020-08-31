using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.JoinTables
{
    public class Class2Group
    {
        public long SchoolClassId { get; set; }
        public SchoolClass SchoolClass { get; set; }

        public long ClassGroupId { get; set; }
        public ClassGroup ClassGroup { get; set; }
    }
}
