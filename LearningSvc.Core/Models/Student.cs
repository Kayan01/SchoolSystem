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
        public long UserId { get; set; }
        public long? ClassId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public SchoolClass Class { get; set; }
    }
}
