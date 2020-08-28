using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.ViewModels.Staff;

namespace Auth.Core.Services.Interfaces
{
   public interface IStaffService
    {
        Task<ResultModel<StaffVM>> AddStaff(StaffVM model);
        Task<ResultModel<List<StaffVM>>> GetAllStaff();
        Task<ResultModel<StaffVM>> GetStaffById(long Id);
        Task<ResultModel<StaffUpdateVM>> UpdateStaff(StaffUpdateVM model );
        Task<ResultModel<bool>> DeleteStaff(long Id);
    }
}
