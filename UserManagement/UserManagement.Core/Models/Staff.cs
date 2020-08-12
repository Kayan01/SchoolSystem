using NPOI.SS.Formula.Functions;
using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Core.Models
{
    public class Staff : AuditedEntity<long>, ITenantModelType
    {
        public long SchoolId { get; set; }

        public string  FirstName { get; set; }
        public string  LastName { get; set; }
    }
}
