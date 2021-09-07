using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Alumni
{
    public  class AlumniEvent : FullAuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
