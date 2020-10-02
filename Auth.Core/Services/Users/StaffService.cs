using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Core.Models;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.Staff;
using System.Linq;
using Auth.Core.Models.Users;
using Shared.Utils;
using Auth.Core.Context;
using Auth.Core.ViewModels;
using Shared.Enums;
using Shared.PubSub;

namespace Auth.Core.Services
{
    public class StaffService : IStaffService
    {
        private readonly IRepository<Staff, long> _staffRepo;
        private readonly IRepository<TeachingStaff, long> _teachingStaffRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthUserManagement _authUserManagement;
        private readonly IPublishService _publishService;
        private readonly AppDbContext _appDbContext;

        public StaffService(IRepository<Staff, long> staffRepo,
            IUnitOfWork unitOfWork,
            IAuthUserManagement authUserManagement,
            IRepository<TeachingStaff, long> teachingStaffRepo,
            IPublishService publishService,
            AppDbContext appDbContext)
        {
            _staffRepo = staffRepo;
            _unitOfWork = unitOfWork;
            _authUserManagement = authUserManagement;
            _teachingStaffRepo = teachingStaffRepo;
            _publishService = publishService;
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
                PhoneNumber = model.PhoneNumber,
                UserType = UserType.Staff
            };

            _unitOfWork.BeginTransaction();

            var authResult = await _authUserManagement.AddUserAsync(userModel);

            if (authResult == null)
            {
                result.AddError("Failed to add authentication for staff");
                return result;
            }

            if (!Enum.TryParse(typeof(StaffType), model.StaffType, out var staffTypeObj))
            {
                result.AddError("Invalid staff type");
                return result;
            }
            var staffType = (StaffType)staffTypeObj;

            var staff = _staffRepo.Insert(new Staff
            {
                UserId = authResult.Value,
                StaffType = staffType
            });

            await _unitOfWork.SaveChangesAsync();

            //check if staff is teacher and adds to teachers table
            if (staffType == StaffType.Teacher)
            {
                _teachingStaffRepo.Insert(new TeachingStaff { Id = staff.Id });
            }

            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.Commit();

            model.Id = staff.Id;
            result.Data = model;

            //TODO Refactor, and Move teachers logic to TeachersService

            //Publish Message
            if (staffType == StaffType.Teacher)
            {
                await _publishService.PublishMessage(Topics.Teacher, BusMessageTypes.TEACHER, new TeacherSharedModel
                {
                    IsActive = true,
                    StaffType = StaffType.Teacher,
                    TenantId = staff.TenantId,
                    UserId = staff.UserId,
                });
            }
            else
            {
                await _publishService.PublishMessage(Topics.Staff, BusMessageTypes.STAFF, new StaffSharedModel
                {
                    IsActive = true,
                    StaffType = staffType,
                    TenantId = staff.TenantId,
                    UserId = staff.UserId,
                });
            }
            return result;
        }

        public async Task<ResultModel<bool>> DeleteStaff(long Id)
        {
            var result = new ResultModel<bool> { Data = false };

            //check if the staff exists
            var staff = await _staffRepo.FirstOrDefaultAsync(Id);

            if (staff == null)
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

            //Publish Message
            await _publishService.PublishMessage(Topics.Teacher, BusMessageTypes.TEACHER, new TeacherSharedModel
            {
                IsActive = false,
                IsDeleted = true,
                StaffType = staff.StaffType,
                TenantId = staff.TenantId,
                UserId = staff.UserId,
            });

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

            //Publish Message
            await _publishService.PublishMessage(Topics.Teacher, BusMessageTypes.TEACHER, new TeacherSharedModel
            {
                IsActive = false,
                IsDeleted = true,
                StaffType = staff.StaffType,
                TenantId = staff.TenantId,
                UserId = staff.UserId,
            });

            return result;
        }
    }
}