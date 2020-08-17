using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.Models;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.Staff;
using System.Linq;

namespace Auth.Core.Services
{
    public class StaffService : IStaffService
    {
        private readonly IRepository<Staff, long> _staffRepo;
        private readonly IUnitOfWork _unitOfWork;
        public StaffService(IRepository<Staff, long> staffRepo, IUnitOfWork unitOfWork)
        {
            _staffRepo = staffRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<object>> GetAllStaff()
        {

            var staffs = new List<Staff>();
            var result = new ResultModel<object>();

            staffs = await _staffRepo.GetAllListAsync();

            if (staffs.Count > 0)
            {
                result.Data = staffs.Select(x => (StaffVM)x);
            }

            return result;
        }

        public async Task<ResultModel<StaffVM>> GetStaffById(long Id)
        {
            var result = new ResultModel<StaffVM>();
            var staff = await _staffRepo.FirstOrDefaultAsync(x => x.Id == Id);

            if (staff == null)
            {
                return result;
            }

            result.Data = staff;
            return result;
        }
        public async Task<ResultModel<StaffVM>> AddStaff(StaffVM model)
        {
            var result = new ResultModel<StaffVM>();
            var staff = _staffRepo.Insert(new Staff { FirstName = model.FirstName, LastName = model.LastName });
            await _unitOfWork.SaveChangesAsync();
            model.Id = staff.Id;
            result.Data = model;
            return result;
        }

        public async Task<ResultModel<bool>> DeleteStaff(long Id)
        {
            var result = new ResultModel<bool> { Data = false };
            await _staffRepo.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            result.Data = true;

            return result;
        }


        public async Task<ResultModel<StaffUpdateVM>> UpdateStaff(StaffUpdateVM model)
        {
            var staff = await _staffRepo.FirstOrDefaultAsync(model.Id);
            var result = new ResultModel<StaffUpdateVM>();

            if (staff != null)
            {
                //TODO: add more props
                staff.FirstName = model.FirstName;
                await _staffRepo.UpdateAsync(staff);
                await _unitOfWork.SaveChangesAsync();
                result.Data = model;
                return result;
            }

            return result;
        }
    }
}
