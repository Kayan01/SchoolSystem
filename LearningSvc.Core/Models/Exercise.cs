using Shared.Entities;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class Exercise : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        //public DateTime? DueDateTime { get; set; }
        //public string Title { get; set; }
        //public string Body { get; set; }

        //[ForeignKey(nameof(FileUpload))]
        //public Guid? AttachmentId { get; set; }

        //public FileUpload Attachment { get; set; }

        //public ICollection<ClassSession> ClassSessions { get; set; }
        //public ICollection<ExerciseAnswer> ExerciseAnswers { get; set; }
    }
}
