using LearningSvc.Core.ViewModels.Teacher;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Interfaces
{
    public interface ITeacherService
    {
        void AddOrUpdateTeacherFromBroadcast(TeacherSharedModel model);
        Task<long> GetTeacherIdByUserId(long userId);
    }
}
