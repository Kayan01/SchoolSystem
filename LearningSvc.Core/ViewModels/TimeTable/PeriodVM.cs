using LearningSvc.Core.Models.TimeTable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LearningSvc.Core.ViewModels.TimeTable
{
    public class PeriodVM
    {
        public long Id { get; set; }
        public int Step { get; set; }
        public string Name { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public TimeSpan TimeFrom { get; set; }
        public string TimeFromS { 
            get {
                return $"{TimeFrom.Hours.ToString("00")}:{TimeFrom.Minutes.ToString("00")}";
            } 
        }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public TimeSpan TimeTo { get; set; }
        public string TimeToS
        {
            get
            {
                return $"{TimeTo.Hours.ToString("00")}:{TimeTo.Minutes.ToString("00")}";
            }
        }

        public bool isBreak { get; set; }

        [NotMapped]
        public double DurationInMinutes
        {
            get
            {
                return (TimeTo - TimeFrom).TotalMinutes;
            }
        }

        public static implicit operator PeriodVM(Period model)
        {
            return model == null ? null : new PeriodVM
            {
                Id = model.Id,
                Name = model.Name,
                TimeFrom = model.TimeFrom,
                TimeTo = model.TimeTo,
                Step = model.Step,
                isBreak = model.IsBreak,
            };
        }
    }
}
