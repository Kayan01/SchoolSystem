using Auth.Core.Models.Subjects;
using Auth.Core.Models.Users;
using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Core.Models.TimeTableSchedule
{
    public class Schedule : AuditedEntity<long>
    {
        public DayOfWeek Day { get; set; }
        public List<Timetable> TimeTable { get; set; }
    }

    public class Timetable
    {
        public long Id { get; set; }
        public int? StaffId { get; set; }
        public int? SubjectId { get; set; }

        /// <summary>
        /// Arranges the
        /// </summary>
        public int PeriodId { get; set; }

        public DateTime StartDate { protected get; set; }
        public DateTime EndDate { protected get; set; }

        [NotMapped]
        public string Duration { get; set; }

        public Subject Subject { get; set; }
        public TeachingStaff TeachingStaff { get; set; }
        public object NoOfLessonPeriod { get; set; }
        public object AllowLesson { get; set; }
        public bool IsLessonPeriod { get; set; }
        public int LevelId { get; set; }
        public int NoOfStudent { get; set; }
        public string PeriodName { get; set; }
        public string PName { get; set; }
        public int RoomTypeId { get; set; }
        public int DayId { get; set; }
        public object DayName { get; set; }
        public int ClassToClassArmId { get; set; }
        public object ClassName { get; set; }
        public object SessionId { get; set; }
        public object TermId { get; set; }
        public object IsActive { get; set; }
        public object CreatedBy { get; set; }
        public bool IsBreakPeriod { get; set; }
        public object CreatedOn { get; set; }
    }
}