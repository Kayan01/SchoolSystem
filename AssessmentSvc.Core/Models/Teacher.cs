using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models
{
    public class Teacher : Person, ITenantModelType
    {
        public long TenantId { get; set; }
        public long? ClassId { get; set; }
        

        public virtual SchoolClass Class { get; set; }

    }
}
