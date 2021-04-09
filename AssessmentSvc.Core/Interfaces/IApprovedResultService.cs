using AssessmentSvc.Core.ViewModels.Result;
using AssessmentSvc.Core.ViewModels.Student;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Interfaces
{
    public interface IApprovedResultService
    {
        Task<ResultModel<string>> SubmitStudentResult(UpdateApprovedStudentResultViewModel vm);
        Task<ResultModel<string>> SubmitClassResultForApproval(UpdateApprovedClassResultViewModel vm);
        Task<ResultModel<GetApprovedStudentResultViewModel>> GetStudentResultForApproval(GetStudentResultForApproval vm);
        Task<ResultModel<List<ResultBroadSheet>>> GetClassTeacherApprovedClassBroadSheet(long classId);
        Task<ResultModel<StudentReportSheetVM>> GetApprovedResultForStudent(long classId, long studentId, long? curSessionId = null, int? termSequenceNumber = null);

        Task<ResultModel<List<StudentReportSheetVM>>> GetApprovedResultForMultipleStudents(long classId, long[] studentIds, long? curSessionId = null, int? termSequenceNumber = null);
        Task<ResultModel<List<StudentVM>>> GetStudentsWithApprovedResult(long classId, long? curSessionId = null, int? termSequenceNumber = null);
        Task<ResultModel<string>> MailResult(MailResultVM vm);
    }
}
