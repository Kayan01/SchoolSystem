using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.ViewModels.Student;
using Shared.Utils;
using Shared.Pagination;
using Microsoft.AspNetCore.Http;

namespace Auth.Core.Services.Interfaces
{
    public interface IStudentService
    {
        Task<ResultModel<StudentVM>> AddStudentToSchool(CreateStudentVM model);

        Task<ResultModel<bool>> DeleteStudent(long Id, string sessionName);

        Task<ResultModel<PaginatedModel<StudentVMs>>> GetAllStudentsInSchool(QueryModel model);
        Task<ResultModel<PaginatedModel<StudentVM>>> GetAllStudentsInClass(QueryModel model, long classId);
        Task<ResultModel<StudentDetailVM>> GetStudentById(long Id);
        Task<ResultModel<StudentVM>> UpdateStudent(long Id, StudentUpdateVM model);
        Task<ResultModel<StudentDetailVM>> GetStudentProfileByUserId(long Id);
        Task<ResultModel<byte[]>> GetStudentsExcelSheet();
        Task<ResultModel<bool>> AddBulkStudent(IFormFile excelfile);
    }
}
