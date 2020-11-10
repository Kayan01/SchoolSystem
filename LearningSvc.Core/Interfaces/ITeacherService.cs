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
        Task<ResultModel<TeacherVM>> AddTeacher(TeacherVM model);
        Task<ResultModel<TeacherVM>> GetTeacherSummaryById(long id);
        Task AddOrUpdateTeacherFromBroadcast(TeacherSharedModel model);
    }
}
