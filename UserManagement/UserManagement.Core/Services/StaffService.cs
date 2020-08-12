using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Models;
using UserManagement.Core.Services.Interfaces;
using UserManagement.Core.ViewModels.Staff;

namespace UserManagement.Core.Services
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
        public async Task<ResultModel<object>> GetAllStaff(long schoolId)
        {
            var result = new ResultModel<object>
            {
                Data = await _staffRepo.GetAllListAsync()
            };
            return result;
        }

        public async Task<ResultModel<StaffVM>> GetStaffById(long Id, long schoolId)
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
        public async Task<ResultModel<StaffVM>> AddStaff(long schoolId, StaffVM model)
        {
            var result = new ResultModel<StaffVM>();
            var staff = _staffRepo.Insert(new Staff {  SchoolId = schoolId,  FirstName = model.FirstName, LastName = model.LastName });
            await _unitOfWork.SaveChangesAsync();
            model.Id = staff.Id;
            result.Data = model;
            return result;
        }

        public async Task<ResultModel<bool>> DeleteStaff(long schoolId, long Id)
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
