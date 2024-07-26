using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class FinanceObJ
    {
        public List<SchoolSharedModel> Schools { get; set; }
        public List<ClassSharedModel> CLasses { get; set; }
        public List<ParentSharedModel> Parents { get; set; }
        public List<StudentSharedModel> Students { get; set; }
    }

}
