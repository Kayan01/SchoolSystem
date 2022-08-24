using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.ViewModels.Staff;
using Shared.Pagination;
using Auth.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Auth.Core.ViewModels.Alumni;

namespace Auth.Core.Services.Interfaces
{
   public interface IAlumniService
    {
        Task<ResultModel<AlumniDetailVM>> AddAlumni(AddAlumniVM vM);
        Task<ResultModel<List<AlumniDetailVM>>> GetAllAlumni(QueryModel model, GetAlumniQueryVM queryVM);
        Task<ResultModel<AlumniDetailVM>> GetAlumniById(long Id);
        Task<ResultModel<AlumniDetailVM>> UpdateAlumni(UpdateAlumniVM model );
        Task<ResultModel<bool>> DeleteAlumni(long Id);
        Task<ResultModel<PastAlumniDetailVM>> AddPastStudents(AddPastAlumniVM model, long schoolId);
        Task<ResultModel<PastAlumniDetailVM>> GetPastAlumniById(long Id);
        Task<ResultModel<List<PastAlumniDetailVM>>> GetAllPastAlumni(QueryModel model, GetAlumniQueryVM queryVM);
    }
}
