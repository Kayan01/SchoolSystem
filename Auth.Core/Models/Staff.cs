using NPOI.SS.Formula.Functions;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models
{
    public class Staff : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        public string  FirstName { get; set; }
        public string  LastName { get; set; }
    }
}
