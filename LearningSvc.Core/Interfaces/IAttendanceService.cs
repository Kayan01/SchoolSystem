using LearningSvc.Core.ViewModels.Attendance;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Interfaces
{
    public interface IAttendanceService
    {
        Task<ResultModel<string>> AddAttendanceForClass(AddClassAttendanceVM model);
        Task<ResultModel<string>> AddAttendanceForSubject(AddSubjectAttendanceVM model);

        Task<ResultModel<List<GetStudentAttendanceSubjectVm>>> GetStudentAttendanceForSubject(
            GetStudentAttendanceSubjectQueryVm vm);

        Task<ResultModel<IEnumerable<ListStudentAttendanceClassVm>>> GetStudentAttendanceForClass(
            GetStudentAttendanceClassQueryVm vm);

        Task<ResultModel<List<StudentAttendanceSummaryVm>>> GetStudentAttendanceSummary(
            long studentId, long classId);

        Task<ResultModel<List<StudentAttendanceReportVM>>> ExportStudentAttendanceReport(AttendanceRequestVM model);
        Task<ResultModel<List<StudentAttendanceReportVM>>> ExportClassAttendanceReport(AttendanceRequestVM model);
        Task<ResultModel<byte[]>> ExportAttendanceDataToExcel(List<StudentAttendanceReportVM> model);
        Task<ResultModel<List<StudentAttendanceReportVM>>> studentSubjectAttendanceView(AttendanceRequestVM model);
    }
}
