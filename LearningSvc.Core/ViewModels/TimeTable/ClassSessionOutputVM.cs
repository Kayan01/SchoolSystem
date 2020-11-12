using LearningSvc.Core.Models.TimeTable;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.TimeTable
{
    public class ClassSessionOutputVM
    {
        public long TimeTableCellId { get; internal set; }
        public string ClassName { get; internal set; }
        public string TeacherName { get; internal set; }
        public string SubjectName { get; internal set; }
        public TimeSpan TimeFrom { get; internal set; }
        public double DurationInMinutes { get; set; }
        public int NoOfPeriods { get; set; } = 1;
        public TimeSpan TimeTo { get {
                return DurationInMinutes == 0d ? TimeSpan.Zero :
                    TimeFrom.Add(TimeSpan.FromMinutes((DurationInMinutes * NoOfPeriods)));
            } }
        public string PeriodName { get; internal set; }
        public int? NoOfStudent { get; set; }
        public bool HasVirtual { get; internal set; }
        public string ZoomId { get; internal set; }

    }
}
