using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models
{
    /// <summary>
    /// Defines the school sections such as Secondary, primary , nursery etc
    /// </summary>
    public class SchoolSection : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string Name { get; set; }
    }
}
