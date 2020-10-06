using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.School;
using Shared.Utils;
using Shared.Pagination;

namespace Auth.Core.Services.Interfaces
{
    public interface ISchoolService
    {
        Task<ResultModel<PaginatedModel<SchoolVM>>> GetAllSchools(QueryModel model);

        Task<ResultModel<SchoolVM>> AddSchool(CreateSchoolVM model);

        Task<ResultModel<SchoolVM>> GetSchoolById(long Id);

        Task<ResultModel<SchoolUpdateVM>> UpdateSchool(SchoolUpdateVM model);

        Task<ResultModel<bool>> DeleteSchool(long Id);
    }
}