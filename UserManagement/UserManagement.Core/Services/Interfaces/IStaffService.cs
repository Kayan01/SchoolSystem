using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.ViewModels.Staff;

namespace UserManagement.Core.Services.Interfaces
{
   public interface IStaffService
    {
        Task<ResultModel<StaffVM>> AddStaff(long schoolId, StaffVM model);
        Task<ResultModel<object>> GetAllStaff(long schoolId);
        Task<ResultModel<StaffVM>> GetStaffById(long Id,long schoolId);
        Task<ResultModel<StaffUpdateVM>> UpdateStaff(StaffUpdateVM model );
        Task<ResultModel<bool>> DeleteStaff(long schoolId,long Id);
    }
}
