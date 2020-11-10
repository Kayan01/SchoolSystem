using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class StudentSharedModel : PersonSharedModel
    {
        public long TenantId { get; set; }
        public long Id { get; set; }
        public long? ClassId { get; set; }
    }
}
