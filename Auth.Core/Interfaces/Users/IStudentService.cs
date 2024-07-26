using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.ViewModels.Student;
using Shared.Utils;
using Shared.Pagination;
using Microsoft.AspNetCore.Http;
using Auth.Core.ViewModels;
using Auth.Core.Models;

namespace Auth.Core.Services.Interfaces
{
    public interface IStudentService
    {
        Task<ResultModel<StudentVM>> AddStudentToSchool(CreateStudentVM model);

        Task<ResultModel<bool>> DeleteStudent(long Id, DeactivateStudent model);

        Task<ResultModel<PaginatedModel<StudentVMs>>> GetAllStudentsInSchool(QueryModel model);
        Task<ResultModel<PaginatedModel<StudentVM>>> GetAllStudentsInClass(QueryModel model, long classId);
        Task<ResultModel<StudentDetailVM>> GetStudentById(long Id);
        Task<ResultModel<StudentVM>> UpdateStudent(long Id, StudentUpdateVM model);
        Task<ResultModel<StudentDetailVM>> GetStudentProfileByUserId(long Id);
        Task<ResultModel<byte[]>> GetStudentsExcelSheet();
        Task<ResultModel<bool>> AddBulkStudent(IFormFile excelfile);
        Task<ResultModel<PaginatedModel<StudentVMs>>> SearchForStudentByName(QueryModel model, string name);
        Task<ResultModel<PaginatedModel<StudentVMs>>> GetStudentByClass(StudentExportVM classVM, QueryModel model);
        Task<ResultModel<List<Student>>> ExportStudentData(StudentExportVM model);
        Task<ResultModel<ExportPayloadVM>> ExportStudentDataExcel(List<Student> model);
        Task<ResultModel<ExportPayloadVM>> ExportStudentDataPDF(List<Student> model);

        Task<ResultModel<FinanceObJ>> GetAllDataNotBroadcasted(DateTime StartDate, DateTime EndDate);
    }
}
