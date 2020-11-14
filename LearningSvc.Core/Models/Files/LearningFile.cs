using LearningSvc.Core.Enumerations;
using Shared.Entities;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.Models
{
    public abstract class LearningFile : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public long SchoolClassSubjectId { get; set; }
        public long TeacherId { get; set; }
        public Guid FileUploadId { get; set; }
        public string OptionalComment { get; set; }

        public Teacher Teacher { get; set; }
        public SchoolClassSubject SchoolClassSubject { get; set; }
        public FileUpload File { get; set; }
    }
}
