using Shared.Entities.Auditing;
using Shared.Enums;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class AttendanceSubject : FullAuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get ; set ; }
        [DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }
        public AttendanceState AttendanceStatus { get; set; }
        public string Remark { get; set; }
        public long SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public SchoolClassSubject SchoolClassSubject { get; set; }
        public long StudentId { get; set; }
        public Student Student { get; set; }
    }
}
