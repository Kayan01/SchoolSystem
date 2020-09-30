using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.ViewModels.Student;
using Shared.Utils;

namespace Auth.Core.Services.Interfaces
{
    public interface IStudentService
    {
        Task<ResultModel<PaginatedList<StudentVM>>> GetAllStudentsInSchool(int pageNumber, int pageSize);
        Task<ResultModel<StudentVM>> AddStudentToSchool(StudentVM model);
        Task<ResultModel<StudentVM>> GetStudentById(long Id);
        Task<ResultModel<StudentUpdateVM>> UpdateStudent(StudentUpdateVM model);
        Task<ResultModel<bool>> DeleteStudent(long Id);
    }
}
