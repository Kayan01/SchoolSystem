using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.ViewModels.Staff;
using Shared.Pagination;
using Auth.Core.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Auth.Core.Services.Interfaces
{
   public interface IAdminService
    {
        Task<ResultModel<AdminListVM>> AddAdmin(AddAdminVM model);
        Task<ResultModel<bool>> BulkAddAdmin(IFormFile model);
        Task<ResultModel<PaginatedModel<AdminListVM>>> GetAllAdmin(QueryModel model);
        Task<ResultModel<AdminDetailVM>> GetAdminById(long Id);
        Task<ResultModel<AdminListVM>> UpdateAdmin(UpdateAdminVM model );
        Task<ResultModel<bool>> DeleteAdmin(long Id);
    }
}
