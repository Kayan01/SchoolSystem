using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LearningSvc.Core.ViewModels.Attendance
{
    public class GetStudentAttendanceClassQueryVm
    {
        public long? StudentId { get; set; }
        public long? StudentUserId { get; set; }
       
        public long? ClassId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class StudentAttendanceReportVM
    {
        public long StudentId { get; set; }
        public string FullName { get; set; }
        public string ClassName { get; set; }
        public int AttendanceStatus { get; set; }
        public int TotalNumberOfTimePresent { get; set; }
        public int TotalNumberOfTimeAbsent { get; set; }
        public string SubjectName { get; set; }

    }

    public class AttendanceExcelReport
    {
        public string Base64String { get; set; }
        public string FileName { get; set; }
    }

    public class AttendanceRequestVM
    {
        public long? tenantId { get; set; }
        public long? ClassId { get; set; }
        public long? SubjectId { get; set; }
        public DateTime? AttendanceStartDate { get; set; }
        public DateTime? AttendanceEndDate { get; set; }
    }
}
