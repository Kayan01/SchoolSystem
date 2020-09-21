using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class Student : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public long? ClassId { get; set; }
        public string Fullname { get; set; }

        public SchoolClass Class { get; set; }
    }
}
