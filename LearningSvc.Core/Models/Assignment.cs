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
        public long SchoolClassId { get; set; }
        public long SubjectId { get; set; }
        public long TeacherId { get; set; }

        public DateTime DueDate { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        [ForeignKey(nameof(FileUpload))]
        public Guid? AttachmentId { get; set; }

        public FileUpload Attachment { get; set; }
        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
        public SchoolClass SchoolClass { get; set; }

        public ICollection<AssignmentAnswer> AssignmentAnswers { get; set; }
    }
}
