using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.School;
using Shared.Utils;
using Shared.Pagination;
using Microsoft.AspNetCore.Http;

namespace Auth.Core.Services.Interfaces
{
    public interface ISchoolService
    {

        Task<ResultModel<int>> GetTotalSchoolsCount();
        Task<ResultModel<SchoolVM>> AddSchool(CreateSchoolVM model);
        Task<ResultModel<bool>> AddBulkSchool(IFormFile model);

        Task<ResultModel<PaginatedModel<SchoolVM>>> GetAllSchools(QueryModel model);
        Task<ResultModel<SchoolVM>> GetSchoolById(long Id);

        Task<ResultModel<SchoolVM>> UpdateSchool(UpdateSchoolVM model);

        Task<ResultModel<bool>> DeleteSchool(long Id);
    }
}