using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LearningSvc.Core.ViewModels.Attendance
{
    public class GetStudentAttendanceClassQueryVm
    {
        public long? StudentId { get; set; }
        [Required]
        public long ClassId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
