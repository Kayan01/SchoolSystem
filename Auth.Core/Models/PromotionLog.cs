using Auth.Core.Enumeration;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Auth.Core.Models
{
    public class PromotionLog : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public PromotionStatus PromotionStatus { get; set; }
        public string Reason { get; set; }

        public long? StudentId { get; set; }
        public long SessionSetupId { get; set; }
        public long? FromClassId { get; set; }
        public long? ToClassId { get; set; }
        public string ClassPoolName { get; set; }

        [ForeignKey("ToClassId")]
        public SchoolClass ToClass { get; set; }
        [ForeignKey("FromClassId")]
        public SchoolClass FromClass { get; set; }
        public Student Student { get; set; }
        public double AverageScore { get; set; }
    }
}
