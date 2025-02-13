﻿using Shared.Entities.Auditing;
using Shared.Enums;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class Subject : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public ICollection<SchoolClassSubject> SchoolClassSubjects { get; set; }
    }
}
