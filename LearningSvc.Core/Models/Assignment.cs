using Shared.Entities;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class Assignment : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public long TeacherId { get; set; }
        public long SchoolClassSubjectId { get; set; }
        public Guid? FileUploadId { get; set; }

        public DateTime DueDate { get; set; }
        public string Title { get; set; }
        public string OptionalComment { get; set; }
        public int TotalScore { get; set; }

        public SchoolClassSubject SchoolClassSubject { get; set; }
        public Teacher Teacher { get; set; }

        public FileUpload Attachment { get; set; }
        public ICollection<AssignmentAnswer> AssignmentAnswers { get; set; }
    }
}
