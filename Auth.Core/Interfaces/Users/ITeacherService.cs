using Auth.Core.ViewModels.Staff;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces.Users
{
    public interface ITeacherService
    {
        Task<ResultModel<TeacherVM>> AddTeacher(AddStaffVM model);
        Task<ResultModel<PaginatedModel<TeacherVM>>> GetTeachers(QueryModel model);
        Task<ResultModel<TeacherDetailVM>> GetTeacherById(long Id);
        Task<ResultModel<ClassTeacherVM>> GetTeacherClassById(long Id);
        Task<ResultModel<TeacherVM>> UpdateTeacher(UpdateTeacherVM model, long Id);
        Task<ResultModel<string>> MakeClassTeacher(ClassTeacherVM model);
        Task<ResultModel<bool>> DeleteTeacher(long userId);
    }
}
