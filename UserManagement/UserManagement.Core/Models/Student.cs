using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Core.Models
{
    public class Student : AuditedEntity<long>, ITenantModelType
    {
        public long SchoolId { get; set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }

       
    }
}
