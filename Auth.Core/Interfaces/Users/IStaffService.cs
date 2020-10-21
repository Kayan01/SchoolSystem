using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.ViewModels.Staff;
using Shared.Pagination;

namespace Auth.Core.Services.Interfaces
{
   public interface IStaffService
    {
        Task<ResultModel<StaffVM>> AddStaff(AddStaffVM model);
        Task<ResultModel<PaginatedModel<StaffVM>>> GetAllStaff(QueryModel model);
        Task<ResultModel<StaffVM>> GetStaffById(long Id);
        Task<ResultModel<StaffVM>> UpdateStaff(StaffUpdateVM model );
        Task<ResultModel<bool>> DeleteStaff(long Id);
    }
}
