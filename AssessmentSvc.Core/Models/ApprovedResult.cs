using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models
{
    public class ApprovedResult : FullAuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string HeadTeacherComment { get; set; }
        public string ClassTeacherComment { get; set; }
        public long StudentId { get; set; }
        public long SessionId { get; set; }
        public int TermSequence { get; set; }
        public long SchoolClassId { get; set; }

        public bool IsClassTeacherApproved { get; set; }
        public bool IsSchoolAdminApproved { get; set; }


        public SessionSetup  SessionSetup { get; set; }
        public Student Student { get; set; }
        public SchoolClass SchoolClass { get; set; }

        public List<Result> Results { get; set; }
    }
}
