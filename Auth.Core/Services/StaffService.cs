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
using Microsoft.EntityFrameworkCore;
using Auth.Core.Models.Users;
using Shared.Utils;

namespace Auth.Core.Services
{
    public class StaffService : IStaffService
    {
        private readonly IRepository<Staff, long> _staffRepo;
        private readonly IRepository<TeachingStaff, long> _teachingStaffRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthUserManagement _authUserManagement;
        public StaffService(IRepository<Staff, long> staffRepo, IUnitOfWork unitOfWork, IAuthUserManagement authUserManagement, IRepository<TeachingStaff, long> teachingStaffRepo)
        {
            _staffRepo = staffRepo;
            _unitOfWork = unitOfWork;
            _authUserManagement = authUserManagement;
            _teachingStaffRepo = teachingStaffRepo;
        }
        public async Task<ResultModel<List<StaffVM>>> GetAllStaff(int pageNumber, int pageSize)
        {
            var pagedData = await PaginatedList<StaffVM>.CreateAsync(_staffRepo.GetAll().Select(x => new StaffVM { Id = x.Id}), pageNumber, pageSize);

            var result = new ResultModel<List<StaffVM>>
            {
                Data = pagedData

            };

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


            //create auth user
            var authResult = await _authUserManagement.AddUserAsync(model.FirstName, model.LastName, model.Email, model.PhoneNumber, model.Password);

            if (authResult == null)
            {
                result.AddError("Failed to add authentication for staff");
                return result;
            }

            var staffType = model.IsTeacher ? Enumerations.StaffType.TeachingStaff : Enumerations.StaffType.NonTeachingStaff;

            var staff = _staffRepo.Insert(new Staff { UserId = authResult.Value, StaffType = staffType });


            await _unitOfWork.SaveChangesAsync();

            //check if staff is teacher and adds to teachers table
            if (model.IsTeacher)
            {
                _teachingStaffRepo.Insert(new TeachingStaff { Id = staff.Id });
            }

            await _unitOfWork.SaveChangesAsync();

            model.Id = staff.Id;
            result.Data = model;
            return result;
        }

        public async Task<ResultModel<bool>> DeleteStaff(long Id)
        {
            var result = new ResultModel<bool> { Data = false };

            //check if the staff exists
            var std = await _staffRepo.FirstOrDefaultAsync(Id);
            if (std == null)
            {
                result.AddError("Staff does not exist");
                return result;
            }


            //delete auth user
            var authResult = await _authUserManagement.DeleteUserAsync((int)Id);

            if (authResult == false)
            {
                result.AddError("Failed to delete authentication for staff");
                return result;
            }

            await _staffRepo.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            result.Data = true;

            return result;
        }


        public async Task<ResultModel<StaffUpdateVM>> UpdateStaff(StaffUpdateVM model)
        {
            var staff = await _staffRepo.FirstOrDefaultAsync(model.Id);
            var result = new ResultModel<StaffUpdateVM>();

            if (staff == null)
            {
                result.AddError("Staff not found");
                return result;
            }



            //update auth user
            var authResult = await _authUserManagement.UpdateUserAsync((int)model.Id, model.FirstName, model.LastName);

            if (authResult == false)
            {
                result.AddError("Failed to update authentication model for staff");
                return result;
            }



            //TODO: add more props
            await _staffRepo.UpdateAsync(staff);
            await _unitOfWork.SaveChangesAsync();
            result.Data = model;
            return result;
        }

    }
}
