using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LearningSvc.Core.ViewModels.TimeTable
{
    public class PeriodInsertVM
    {
        public int Step { get; set; }
        public string Name { get; set; }

        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Allowed Time format, 'HH:MM'.")]
        public string TimeFrom { get; set; }

        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Allowed Time format, 'HH:MM'.")]
        public string TimeTo { get; set; }

        public bool isBreak { get; set; }

    }
}
