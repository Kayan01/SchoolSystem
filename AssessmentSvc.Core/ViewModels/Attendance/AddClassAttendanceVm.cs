using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Attendance
{
    public class AddSubjectAttendanceVM
    {
        public long SubjectId { get; set; }
        public DateTime Date { get; set; }
        public List<StudentAttendanceVm> StudentAttendanceVMs { get; set; }
    }
    public class AddClassAttendanceVm
    {
        public long ClassId { get; set; }
        public DateTime Date { get; set; }
        public List<StudentAttendanceVm> StudentAttendanceVm { get; set; }
    }

    public class StudentAttendanceVm
    {
        public long StudentId { get; set; }
        public AttendanceState AttendanceStatus { get; set; }
        public string Remark { get; set; }
    }

    public class StudentAttendanceSummaryVm
    {
        public long StudentId { get; set; }
        public long ClassId { get; set; }
        public int NoOfTimesPresent { get; set; }
        public int NoOfTimesAbsent { get; set; }
        public int TotalNoOfSchoolDays => NoOfTimesPresent + NoOfTimesAbsent;

    }
}
