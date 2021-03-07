using AssessmentSvc.Core.ViewModels.Student;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Interfaces
{
    public interface IStudentService
    {
        void AddOrUpdateStudentFromBroadcast(StudentSharedModel model);
        Task<long> GetStudentClassIdByUserId(long userId);
        Task<ResultModel<List<StudentVM>>> GetStudentsByClass(long classId);
        Task<ResultModel<List<StudentParentMailingInfo>>> GetParentsMailInfo(long[] ids);
    }
}
