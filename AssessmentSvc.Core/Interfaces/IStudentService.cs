using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Interfaces
{
    public interface IStudentService
    {
        Task AddOrUpdateStudentFromBroadcast(StudentSharedModel model);
        Task<long> GetStudentClassIdByUserId(long userId);
    }
}
