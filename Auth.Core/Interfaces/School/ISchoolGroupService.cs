using Shared.ViewModels;
using System.Threading.Tasks;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.SchoolGroup;
using Shared.Pagination;
using Microsoft.AspNetCore.Http;

namespace Auth.Core.Services.Interfaces
{
    public interface ISchoolGroupService
    {
        Task<ResultModel<SchoolGroupListVM>> AddSchoolGroup(CreateSchoolGroupVM model);
        Task<ResultModel<bool>> AddBulkSchoolGroup(IFormFile model);
        Task<ResultModel<PaginatedModel<SchoolVM>>> GetAllSchoolInGroup(QueryModel model, long id);
        Task<ResultModel<PaginatedModel<SchoolGroupListVM>>> GetAllSchoolGroups(QueryModel model, long? id);
        Task<ResultModel<SchoolGroupListVM>> UpdateSchoolGroup(UpdateSchoolGroupVM model, long Id);
    }
}