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
        Task<ResultModel<byte[]>> GetSchoolExcelSheet();
        Task<ResultModel<int>> GetTotalSchoolsCount();
        Task<ResultModel<SchoolVM>> AddSchool(CreateSchoolVM model);
        Task<ResultModel<bool>> AddBulkSchool(IFormFile model);
        Task<ResultModel<PaginatedModel<SchoolVM>>> GetAllSchools(QueryModel model, long? groupId = null);
        Task<ResultModel<SchoolDetailVM>> GetSchoolById(long Id);
        Task<ResultModel<SchoolNameAndLogoVM>> GetSchoolNameAndLogoById(long Id);
        Task<ResultModel<SchoolNameAndLogoVM>> GetSchoolNameAndLogoByDomain(string domain);
        Task<ResultModel<SchoolVM>> UpdateSchool(UpdateSchoolVM model, long Id);
        Task<ResultModel<bool>> DeleteSchool(long Id);
        Task<ResultModel<bool>> CheckSchoolDomain(CreateSchoolVM model);
        Task<ResultModel<bool>> DeActivateSchool(long Id);
        Task<ResultModel<bool>> ActivateSchool(long Id);
    }
}