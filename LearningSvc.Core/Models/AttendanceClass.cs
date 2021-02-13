using LearningSvc.Core.Enumerations;
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
    public class AttendanceClass : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        [DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }
        public long StudentId { get; set; }
        public AttendanceState AttendanceStatus { get; set; }
        public string Remark { get; set; }
        public long ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public SchoolClass SchoolClass { get; set; }
        public Student Student { get; set; }
    }
}
