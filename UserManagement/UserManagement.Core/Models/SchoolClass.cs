using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Core.Models
{
    public class SchoolClass:AuditedEntity<long>, ITenantModelType
    {
        public ICollection<Student> Students { get; set; }
        public long SchoolId { get; set; }
        public string Name { get; set; }
    }
}
