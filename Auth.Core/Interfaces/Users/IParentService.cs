using Auth.Core.ViewModels.Parent;
using Auth.Core.ViewModels.School;
using Auth.Core.ViewModels.Student;
using IPagedList;
using Microsoft.AspNetCore.Http;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces.Users
{
    public interface IParentService
    {
        Task<ResultModel<PaginatedModel<ParentListVM>>> GetAllParents(QueryModel vm);
        Task<ResultModel<ParentDetailVM>> GetParentsForStudent(long studId);
        Task<ResultModel<PaginatedModel<ParentListVM>>> GetAllParentsInSchool(long schoolId,QueryModel vm);
        Task<ResultModel<ParentDetailVM>> GetParentById(long Id);
        Task<ResultModel<List<SchoolParentViewModel>>> GetStudentsSchools(long currentUserId);
        Task<ResultModel<List<StudentParentVM>>> GetStudentsInSchool(long parentId);
        Task<ResultModel<ParentDetailVM>> AddNewParent(AddParentVM vm);
        Task<ResultModel<ParentDetailVM>> UpdateParent(long Id,UpdateParentVM vm);
        Task<ResultModel<string>> DeleteParent(long Id);
        Task<ResultModel<byte[]>> GetParentExcelSheet();
        Task<ResultModel<bool>> UploadBulkParentData(IFormFile excelfile);
        Task<ResultModel<PaginatedModel<ParentListVM>>> GetParentByName(QueryModel vm, string FirstName);
        Task<ResultModel<PaginatedModel<ParentListVM>>> GetParentBySchoolAndName(QueryModel vm, SearchParentVm model);
    }
}
