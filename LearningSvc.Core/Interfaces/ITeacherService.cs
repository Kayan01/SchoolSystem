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
        Task<ResultModel<List<TeacherVM>>> GetAllTeacher();
        ResultModel<TeacherVM> AddTeacher(TeacherVM model);
        Task<ResultModel<TeacherVM>> GetTeacherSummaryById(long id);
        void AddOrUpdateTeacherFromBroadcast(TeacherSharedModel model);
    }
}
