using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.Attendance
{
    public class GetStudentAttendanceSubjectVm
    {
        public string SubjectName { get; set; }
        public int NoOfTImesHeld { get; set; }
        public int NoOfTimesAttended { get; set; }
        public double Percentage { get; set; }
    }
}
