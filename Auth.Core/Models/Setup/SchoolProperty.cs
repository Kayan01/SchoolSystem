using Shared.Entities.Auditing;
using Shared.Enums;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Setup
{
    public class SchoolProperty : FullAuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        public string Prefix { get; set; }
        public string Seperator { get; set; }
        public long EnrollmentAmount { get; set; }
        public int NumberOfTerms { get; set; }
        public ClassDaysType ClassDays { get; set; }


    }
}
