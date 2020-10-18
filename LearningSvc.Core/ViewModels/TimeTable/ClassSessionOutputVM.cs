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
        public TimeSpan? TimeFrom { get; internal set; }
        public TimeSpan? TimeTo { get; internal set; }
        public string PeriodName { get; internal set; }
        public int? NoOfStudent { get; set; }
        public bool HasVirtual { get; internal set; }
        public string ZoomId { get; internal set; }


        public static implicit operator ClassSessionOutputVM(TimeTableCell model)
        {
            return model == null ? null : new ClassSessionOutputVM
            {
                TimeTableCellId = model.Id,
                ClassName = model.SchoolClass?.Name,
                SubjectName = model.Subject?.Name,
                TeacherName = ($"{model.Teacher?.FirstName} {model.Teacher?.LastName}").Trim(),
                TimeFrom = model.Period?.TimeFrom,
                TimeTo = model.Period == null ? null : 
                    model.Period?.TimeFrom.Add(TimeSpan.FromMinutes((model.Period?.DurationInMinutes * model.NoOfPeriod).Value)),
                PeriodName = model.Period?.Name,
                NoOfStudent = model.SchoolClass?.Students?.Count,
                HasVirtual = model.HasVirtual,
                ZoomId = model.ZoomId
            };
        }
    }
}
