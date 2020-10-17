using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;

namespace LearningSvc.Core.Models.TimeTable
{
    public class Period : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string Name { get; set; }
        public int Step { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }

        public ICollection<TimeTableCell> TimeTableCells { get; set; }

        public double DurationInMinutes { get {
                return (TimeTo - TimeFrom).TotalMinutes;
            } }
    }
}