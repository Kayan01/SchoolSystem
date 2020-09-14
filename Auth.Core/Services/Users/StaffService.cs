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
using Auth.Core.Context;
using Auth.Core.ViewModels;

namespace Auth.Core.Services
{
    public class StaffService : IStaffService
    {
        private readonly IRepository<Staff, long> _staffRepo;
        private readonly IRepository<TeachingStaff, long> _teachingStaffRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthUserManagement _authUserManagement;
        private readonly AppDbContext _appDbContext;

        public StaffService(IRepository<Staff, long> staffRepo, IUnitOfWork unitOfWork, IAuthUserManagement authUserManagement, IRepository<TeachingStaff, long> teachingStaffRepo, AppDbContext appDbContext)
        {
            _staffRepo = staffRepo;
            _unitOfWork = unitOfWork;
            _authUserManagement = authUserManagement;
            _teachingStaffRepo = teachingStaffRepo;
            _appDbContext = appDbContext;
        }

        public async Task<ResultModel<List<StaffVM>>> GetAllStaff(int pageNumber, int pageSize)
        {
            //use appdbcontext directly so that we can do a join with the auth users table
            var query = _appDbContext.Staffs.Join(
               _appDbContext.Users, student => student.UserId, authUser => authUser.Id,
               (student, authUser) => new StaffVM
               {
                   FirstName = authUser.FirstName,
                   LastName = authUser.LastName,
                   Email = authUser.Email,
                   PhoneNumber = authUser.PhoneNumber
               });

            var pagedData = await PaginatedList<StaffVM>.CreateAsync(query, pageNumber, pageSize);

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
            var userModel = new AuthUserModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber
            };

            var authResult = await _authUserManagement.AddUserAsync(userModel);

            if (authResult == null)
            {
                result.AddError("Failed to add authentication for staff");
                return result;
            }

            var staffType = model.IsTeacher ? Enumerations.StaffType.TeachingStaff : Enumerations.StaffType.NonTeachingStaff;

            var staff = _staffRepo.Insert(new Staff
            {
                UserId = authResult.Value,
                StaffType = staffType
            });

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
            var result = new ResultModel<StaffUpdateVM>();

            var staff = await _staffRepo.FirstOrDefaultAsync(model.Id);

            if (staff == null)
            {
                result.AddError("Staff not found");
                return result;
            }

            //update auth user
            var userModel = new AuthUserModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var authResult = await _authUserManagement.UpdateUserAsync((int)model.Id, userModel);

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