using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class SubjectSharedModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public long TenantId { get; set; }
    }
}
