using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.Attendance
{
    public class GetStudentAttendanceClassQueryVm
    {
        public long StudentId { get; set; }
        public long ClassId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
