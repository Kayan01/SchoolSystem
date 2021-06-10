using System;
using System.Collections.Generic;
using System.Text;
using Shared.Enums;

namespace LearningSvc.Core.ViewModels.Attendance
{
    public class ListStudentAttendanceClassVm
    {
        public long StudentId { get; set; }
        public IEnumerable<GetStudentAttendanceClassVm>  AttendanceClassVms { get; set; }
    }
    public class GetStudentAttendanceClassVm
    {
        public DateTime AttendanceDate { get; set; }
        public  AttendanceState AttendanceStatus { get; set; }
        public string Reason { get; set; }
    }
}
