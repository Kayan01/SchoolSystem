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
        public DayOfWeek Day { get; set; }

        public long TeacherId { get; set; }
        public long SubjectId { get; set; }
        public long SchoolClassId { get; set; }

        public int NoOfPeriod { get; set; } = 1;
        public bool HasVirtual { get; set; } = false;
        public string ZoomId { get; set; }

        //Start period
        public Period Period { get; set; }
        public Subject Subject { get; set; }
        public Teacher Teacher { get; set; }
        public SchoolClass SchoolClass { get; set; }

    }
}