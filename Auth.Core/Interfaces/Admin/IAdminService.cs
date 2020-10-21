using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.ViewModels.Staff;
using Shared.Pagination;
using Auth.Core.ViewModels;

namespace Auth.Core.Services.Interfaces
{
   public interface IAdminService
    {
        Task<ResultModel<AdminVM>> AddAdmin(AddAdminVM model);
        Task<ResultModel<PaginatedModel<AdminVM>>> GetAllAdmin(QueryModel model);
        Task<ResultModel<AdminVM>> GetAdminById(long Id);
        Task<ResultModel<AdminVM>> UpdateAdmin(UpdateAdminVM model );
        Task<ResultModel<bool>> DeleteAdmin(long Id);
    }
}
