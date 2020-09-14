using LearningSvc.Core.Enumerations;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Shared.Entities;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class ClassSession: AuditedEntity<Guid>, ITenantModelType
    {
        public long TenantId { get; set; }
        public DateTime? DateAndTime { get; set; }
        public long ClassId { get; set; }
        public long SubjectId { get; set; }
        public long TeacherId { get; set; }
        public long ExeciseId { get; set; }
        public SessionState SessionState { get; set; }
        public bool EnableVirtual { get; set; }

        [ForeignKey(nameof(FileUpload))]
        public Guid LessonNoteId { get; set; }


        public FileUpload LessonNote { get; set; }

        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
        public SchoolClass Class { get; set; }
        public Exercise Exercise { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<ExerciseAnswer> ExerciseAnswers { get; set; }
    }
}
