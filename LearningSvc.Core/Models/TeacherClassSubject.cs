using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class TeacherClassSubject : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        public long TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public long SchoolClassSubjectId { get; set; }
        public SchoolClassSubject SchoolClassSubject { get; set; }
    }
}
