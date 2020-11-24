using LearningSvc.Core.Enumerations;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningSvc.Core.Models.TimeTable
{

    public class TimeTableCell : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        public long PeriodId { get; set; }
        public WeekDays Day { get; set; }

        public long TeacherClassSubjectId { get; set; }

        public int NoOfPeriod { get; set; } = 1;
        public bool HasVirtual { get; set; } = false;

        //Start period
        public Period Period { get; set; }
        public TeacherClassSubject TeacherClassSubject { get; set; }

    }
}