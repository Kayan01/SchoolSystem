using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core
{
    public abstract class Person : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public long UserId { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}