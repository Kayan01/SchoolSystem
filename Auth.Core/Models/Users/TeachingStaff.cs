using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Auth.Core.Models.Users
{
    public class TeachingStaff : FullAuditedEntity<long>, ITenantModelType
    {
        [Key, ForeignKey(nameof(Staff))]
        public override long Id { get; set; }
        public long? ClassId { get; set; }
        public SchoolClass Class { get; set; }
        public long TenantId { get; set; }

        public Staff Staff { get; set; }


    }
}
