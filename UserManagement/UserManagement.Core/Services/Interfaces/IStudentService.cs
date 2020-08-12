using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.ViewModels.Student;

namespace UserManagement.Core.Services.Interfaces
{
    public interface IStudentService
    {
        Task<ResultModel<object>> GetAllStudentsInSchool(long schoolId);
        Task<ResultModel<StudentVM>> AddStudentToSchool(long schoolId, StudentVM model);
        Task<ResultModel<StudentVM>> GetStudentById(long Id, long schoolId);
        Task<ResultModel<StudentUpdateVM>> UpdateStudent(StudentUpdateVM model);
        Task<ResultModel<bool>> DeleteStudent(long Id);
    }
}
