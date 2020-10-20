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
        public DayOfWeek Day { get; set; }

        public long TeacherId { get; set; }
        public string TeacherName { get; set; }
        public long SubjectId { get; set; }
        public string SubjectName { get; set; }
        public long SchoolClassId { get; set; }
        public string ClassName { get; set; }

        public int NoOfPeriod { get; set; } = 1;
        public bool HasVirtual { get; set; } = false;
        public string ZoomId { get; set; }


        public static implicit operator TimeTableCellVM(TimeTableCell model)
        {
            return model == null ? null : new TimeTableCellVM
            {
                Id = model.Id,
                PeriodId = model.PeriodId,
                Day = model.Day,
                TeacherId = model.TeacherId,
                TeacherName = ($"{model.Teacher?.FirstName} {model.Teacher?.LastName}").Trim(),
                SubjectId = model.SubjectId,
                SubjectName = model.Subject?.Name,
                SchoolClassId = model.SchoolClassId,
                ClassName = model.SchoolClass?.Name,
                NoOfPeriod = model.NoOfPeriod,
                HasVirtual = model.HasVirtual,
                ZoomId = model.ZoomId,

            };
        }
    }
}
