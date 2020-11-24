using LearningSvc.Core.Enumerations;
using LearningSvc.Core.Models.TimeTable;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.TimeTable
{
    public class TimeTableCellVM
    {
        public long Id { get; set; }

        public long PeriodId { get; set; }
        public string PeriodName { get; set; }
        public WeekDays Day { get; set; }

        public long TeacherClassSubjectId { get; set; }

        public long TeacherId { get; set; }
        public string TeacherName { get; set; }
        public long SubjectId { get; set; }
        public string SubjectName { get; set; }
        public long SchoolClassId { get; set; }
        public string ClassName { get; set; }

        public int NoOfPeriod { get; set; } = 1;
        public bool HasVirtual { get; set; } = false;
        public string ZoomId { get; set; }


    }
}
