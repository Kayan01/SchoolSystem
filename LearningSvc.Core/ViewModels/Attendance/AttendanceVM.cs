using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.Attendance
{
    public class AddSubjectAttendanceVM
    {
        public long SubjectId { get; set; }
        public DateTime Date { get; set; }
        public List<StudentAttendanceVM> StudentAttendanceVMs { get; set; }
    }
    public class AddClassAttendanceVM
    {
        public long ClassId { get; set; }
        public DateTime Date { get; set; }
        public List<StudentAttendanceVM> StudentAttendanceVMs { get; set; }
    }

    public class StudentAttendanceVM
    {
        public long StudentId { get; set; }
        public AttendanceState AttendanceStatus { get; set; }
        public string Remark { get; set; }
    }
}
