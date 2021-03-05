using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LearningSvc.Core.ViewModels.Attendance
{
    public class GetStudentAttendanceSubjectQueryVm
    {
        [Required]
        public long StudentId { get; set; }
        public long? SubjectId { get; set; }
        public DateTime? Date { get; set; }
    }
}
