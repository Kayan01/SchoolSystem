using LearningSvc.Core.ViewModels.Subject;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Interfaces
{
    public interface IStudentService
    {
        void AddOrUpdateStudentFromBroadcast(StudentSharedModel model);
        Task<long> GetStudentClassIdByUserId(long userId);
    }
}
