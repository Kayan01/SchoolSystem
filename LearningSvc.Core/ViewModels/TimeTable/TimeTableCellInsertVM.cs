using LearningSvc.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LearningSvc.Core.ViewModels.TimeTable
{
    public class TimeTableCellInsertVM
    {
        [Required]
        public long PeriodId { get; set; }
        [Required]
        public WeekDays Day { get; set; }

        [Required]
        public long TeacherClassSubjectId { get; set; }

        public bool HasVirtual { get; set; } = false;

    }
}
