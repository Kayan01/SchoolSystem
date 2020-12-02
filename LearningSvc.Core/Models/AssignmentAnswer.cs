using Shared.Entities;
using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningSvc.Core.Models
{
    public class AssignmentAnswer : AuditedEntity<long>
    {
        public long AssignmentId { get; set; }
        public long StudentId { get; set; }
        public string Comment { get; set; }
        public double Score { get; set; }
        public Guid FileUploadId { get; set; }

        public FileUpload Attachment { get; set; }
        public Student Student { get; set; }
        public Assignment Assignment { get; set; }

    }
}