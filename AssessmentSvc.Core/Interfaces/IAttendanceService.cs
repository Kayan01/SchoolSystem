using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.ViewModels.Attendance;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Interfaces
{
    public interface IAttendanceService
    {
        void AddOrUpdateAttendanceForClassFromBroadCast(ClassAttendanceSharedModel model);
        void AddOrUpdateAttendanceForSubjectFromBroadCast(AttendanceSubject model);
        Task<ResultModel<List<StudentAttendanceSummaryVm>>> GetStudentAttendanceSummary(
            long studentId, long classId);
    }
}
