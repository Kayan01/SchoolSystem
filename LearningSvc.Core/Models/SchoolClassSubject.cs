﻿using LearningSvc.Core.Models.Files;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class SchoolClassSubject : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        public long SchoolClassId { get; set; }
        public SchoolClass SchoolClass { get; set; }

        public long SubjectId { get; set; }
        public Subject Subject { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Classwork> Classworks { get; set; }
        public ICollection<LessonNote> LessonNotes { get; set; }
        public ICollection<Media> Medias { get; set; }
    }
}
