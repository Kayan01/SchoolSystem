using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.ViewModels;

namespace Auth.Core.Services.Interfaces
{
   public interface ISchoolService
    {
        Task<ResultModel<List<SchoolVM>>> GetAllSchools(int pageNumber, int pageSize);
        Task<ResultModel<SchoolVM>> AddSchool(SchoolVM model);
        Task<ResultModel<SchoolVM>> GetSchoolById(long Id);
        Task<ResultModel<SchoolUpdateVM>> UpdateSchool(SchoolUpdateVM model);
        Task<ResultModel<bool>> DeleteSchool(long Id);
    }
}
