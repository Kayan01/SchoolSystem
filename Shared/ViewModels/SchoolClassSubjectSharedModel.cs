using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class SchoolClassSubjectSharedModel
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public long SchoolClassId { get; set; }
        public long SubjectId { get; set; }
    }
}
