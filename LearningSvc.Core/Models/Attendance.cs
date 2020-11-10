using LearningSvc.Core.Enumerations;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class Attendance : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        //public DateTime AttendanceDate { get; set; }
        //public long StudentId { get; set; }
        //public AttendanceState AttendanceStatus { get; set; }
        //public string Remark { get; set; }

        //public Student Student { get; set; }
    }
}
