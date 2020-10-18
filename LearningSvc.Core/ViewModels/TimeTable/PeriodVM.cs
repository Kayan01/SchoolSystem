using LearningSvc.Core.Models.TimeTable;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.TimeTable
{
    public class PeriodVM
    {
        public long Id { get; set; }
        public int Step { get; set; }
        public string Name { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }

        public static implicit operator PeriodVM(Period model)
        {
            return model == null ? null : new PeriodVM
            {
                Id = model.Id,
                Name = model.Name,
                TimeFrom = model.TimeFrom,
                TimeTo = model.TimeTo,
                Step = model.Step,
            };
        }
    }
}
