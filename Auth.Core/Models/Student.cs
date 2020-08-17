using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models
{
    public class Student : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }

       
    }
}
