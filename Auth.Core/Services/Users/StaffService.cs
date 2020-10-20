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
using Microsoft.EntityFrameworkCore;
using Shared.Pagination;
using Microsoft.AspNetCore.Identity;
using Shared.Entities;

namespace Auth.Core.Services
{
    public class StaffService : IStaffService
    {
        private readonly IRepository<Staff, long> _staffRepo;
        private readonly IRepository<TeachingStaff, long> _teachingStaffRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IPublishService _publishService;
        private readonly AppDbContext _appDbContext;

        public StaffService(IRepository<Staff, long> staffRepo,
            IUnitOfWork unitOfWork,
            UserManager<User> userManagement,
            IRepository<TeachingStaff, long> teachingStaffRepo,
            IPublishService publishService,
            AppDbContext appDbContext)
        {
            _staffRepo = staffRepo;
            _unitOfWork = unitOfWork;
            _userManager = userManagement;
            _teachingStaffRepo = teachingStaffRepo;
            _publishService = publishService;
            _appDbContext = appDbContext;
        }

        public async Task<ResultModel<PaginatedModel<StaffVM>>> GetAllStaff(QueryModel model)
        {
            var result = new ResultModel<PaginatedModel<StaffVM>>();
            //use appdbcontext directly so that we can do a join with the auth users table
            var query = _staffRepo.GetAll()
                .Include(x => x.User).Select( x=> new StaffVM
                {
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber
                });



            var totalCount = query.Count();
            var pagedData = await PaginatedList<StaffVM>.CreateAsync(query, model.PageIndex, model.PageSize);

            result.Data = new PaginatedModel<StaffVM>(pagedData.Select(x => x), model.PageIndex, model.PageSize, totalCount);

            return result;
        }

        public async Task<ResultModel<StaffVM>> GetStaffById(long Id)
        {
            var result = new ResultModel<StaffVM>();
            var staff = await _staffRepo.GetAll()
                            .Include(x => x.User)
                            .FirstOrDefaultAsync(x => x.UserId == Id);


            result.Data = staff;
            return result;
        }

        public async Task<ResultModel<StaffVM>> AddStaff(AddStaffVM model)
        {
            var result = new ResultModel<StaffVM>();

            //create auth user
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserType = UserType.Staff,
            };
            var userResult = await _userManager.CreateAsync(user, model.Password);

            if (!userResult.Succeeded)
            {
                result.AddError(string.Join(';', userResult.Errors.Select(x => x.Description)));
                return result;
            }


            var staff = _staffRepo.Insert(new Staff
            {
                UserId = user.Id,
                StaffType = StaffType.Teacher,
                //TenantId TODO for some reason Tenant Id is not set for this item

            });

            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.Commit();

            model.Id = staff.Id;

            result.Data = new StaffVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            }; ;

            //TODO Refactor, and Move teachers logic to TeachersService

            //Publish Message
            await _publishService.PublishMessage(Topics.Staff, BusMessageTypes.STAFF, new StaffSharedModel
                {
                    IsActive = true,
                    StaffType = staff.StaffType,
                    TenantId = staff.TenantId,
                    UserId = staff.UserId,
                });
            
            return result;
        }

        public async Task<ResultModel<bool>> DeleteStaff(long Id)
        {
            var result = new ResultModel<bool> { Data = false };

            var staff = await _staffRepo.GetAllIncluding(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == Id);

            if (staff == null)
            {
                result.AddError($"Staff not found");
                return result;
            }
            //TODO disable teacher

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

        public async Task<ResultModel<StaffVM>> UpdateStaff(StaffUpdateVM model)
        {
            var result = new ResultModel<StaffVM>();

            var staff = await _staffRepo.GetAll()
                            .Include(x => x.User)
                            .FirstOrDefaultAsync(x => x.UserId == model.Id);

            if (staff == null)
            {
                result.AddError($"Staff not found");
                return result;
            }

            _unitOfWork.BeginTransaction();

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                result.AddError("User not found");
                return result;
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            await _userManager.UpdateAsync(user);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();


            //Publish Message
            await _publishService.PublishMessage(Topics.Staff, BusMessageTypes.STAFF_UPDATE, new TeacherSharedModel
            {
                IsActive = true,
                IsDeleted = false,
                StaffType = staff.StaffType,
                TenantId = staff.TenantId,
                UserId = staff.UserId,
            });

            result.Data = staff;
            return result;
        }
    }
}