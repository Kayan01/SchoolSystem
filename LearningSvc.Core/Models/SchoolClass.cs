using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class SchoolClass : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string Name { get; set; }
        public string ClassArm { get; set; }

        public string ZoomRoomId { get; set; }

        public ICollection<Student> Students { get; set; }
        public ICollection<SchoolClassSubject> SchoolClassSubjects { get; set; }
    }
}
