using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.ViewModels.Student;
using Shared.Utils;
using Shared.Pagination;

namespace Auth.Core.Services.Interfaces
{
    public interface IStudentService
    {
        Task<ResultModel<StudentVM>> AddStudentToSchool(CreateStudentVM model);

        Task<ResultModel<bool>> DeleteStudent(long Id);

        Task<ResultModel<PaginatedModel<StudentVM>>> GetAllStudentsInSchool(QueryModel model);
        Task<ResultModel<PaginatedModel<StudentVM>>> GetAllStudentsInClass(QueryModel model, long classId);
        Task<ResultModel<StudentDetailVM>> GetStudentById(long Id);
        Task<ResultModel<StudentVM>> UpdateStudent(long Id, StudentUpdateVM model);
        Task<ResultModel<StudentDetailVM>> GetStudentProfileById(long Id);
    }
}
