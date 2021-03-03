using LearningSvc.Core.ViewModels.Attendance;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
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

        Task<ResultModel<List<GetStudentAttendanceClassVm>>> GetStudentAttendanceForClass(
            GetStudentAttendanceClassQueryVm vm);
    }
}
