using Auth.Core.Context;
using Auth.Core.Interfaces.Users;
using Auth.Core.Models.Users;
using Auth.Core.ViewModels.Staff;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Entities;
using Shared.Enums;
using Shared.Pagination;
using Shared.PubSub;
using Shared.Utils;
using Shared.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Core.Services.Users
{
    public class TeacherService : ITeacherService
    {
        private readonly IRepository<TeachingStaff, long> _teacherRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IPublishService _publishService;
        private readonly AppDbContext _appDbContext;

        public TeacherService(UserManager<User> userManager,
            IRepository<TeachingStaff, long> teacherRepo,
            IUnitOfWork unitOfWork,
            IPublishService publishService,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _appDbContext = appDbContext;
            _teacherRepo = teacherRepo;
            _publishService = publishService;
        }

        public async Task<ResultModel<PaginatedModel<TeacherVM>>> GetTeachers(QueryModel model)
        {
            var query = _teacherRepo.GetAll()
                          .Include(x => x.Class)
                          .Include(x => x.Staff.User);

            var totalCount = query.Count();
            var pagedData = await PaginatedList<TeachingStaff>.CreateAsync(query, model.PageIndex, model.PageSize);

            return new ResultModel<PaginatedModel<TeacherVM>>(
                new PaginatedModel<TeacherVM>(pagedData.Select(x => (TeacherVM)x), model.PageIndex, model.PageSize, totalCount));
            
        }

        public async Task<ResultModel<TeacherVM>> GetTeacherByUserId(long userId)
        {
            var result = new ResultModel<TeacherVM>();
            var query = _teacherRepo.GetAll()
                            .Include(x => x.Staff.User)
                            .Include(x => x.Class)
                            .FirstOrDefault(x => x.Staff.UserId == userId);

            return new ResultModel<TeacherVM>(query);
        }

        public async Task<ResultModel<TeacherVM>> AddTeacher(AddTeacherVM model)
        {
            var result = new ResultModel<TeacherVM>();

            _unitOfWork.BeginTransaction();

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

            var teacher = _teacherRepo.Insert(new TeachingStaff
            {
                ClassId = model.ClassId,
                Staff = new Models.Staff
                {
                    UserId = user.Id,
                    StaffType = StaffType.Teacher, 
                    //TenantId TODO for some reason Tenant Id is not set for this item
                }
            });

            teacher.Staff.TenantId = teacher.TenantId;//TODO remove this when the tenant Id is automatically added to Staff


            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.Commit();

            await _publishService.PublishMessage(Topics.Teacher, BusMessageTypes.TEACHER, new TeacherSharedModel
            {
                Id = teacher.Id,
                IsActive = true,
                StaffType = StaffType.Teacher,
                TenantId = teacher.TenantId,
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                ClassId = teacher.ClassId
            });

            result.Data = new TeacherVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ClassId = teacher.ClassId,
            };

            return result;

        }

        public async Task<ResultModel<TeacherVM>> UpdateTeacher(UpdateTeacherVM model)
        {
            var result = new ResultModel<TeacherVM>();

            var teacher = _teacherRepo.GetAll()
                            .Include(x => x.Staff.User)
                            .Include(x => x.Class)
                            .FirstOrDefault(x => x.Staff.UserId == model.UserId);

            if (teacher == null)
            {
                result.AddError($"Teacher not found");
                return result;
            }

            _unitOfWork.BeginTransaction();
            teacher.ClassId = model.ClassId;

            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
            {
                result.AddError("User not found");
                return result;
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;

            await _userManager.UpdateAsync(user);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();

            await _publishService.PublishMessage(Topics.Teacher, BusMessageTypes.TEACHER_UPDATE, new TeacherSharedModel
            {
                Id = teacher.Id,
                IsActive = true,
                StaffType = StaffType.Teacher,
                TenantId = teacher.TenantId,
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                ClassId = teacher.ClassId
            });

            result.Data = teacher;
            return result;
        }

        public async Task<ResultModel<bool>> DeleteTeacher(long userId)
        {
            //TODO add IS active Status to Staff or Teacher
            var result = new ResultModel<bool>();

            var teacher = await _teacherRepo.GetAllIncluding(x => x.Staff.User).FirstOrDefaultAsync(x => x.Staff.UserId == userId);

            if (teacher == null)
            {
                result.AddError($"Teacher not found");
                return result;
            }
            //TODO disable teacher
            
            _unitOfWork.SaveChanges();
            await _publishService.PublishMessage(Topics.Teacher, BusMessageTypes.TEACHER_DELETE, new TeacherSharedModel
            {
                Id = teacher.Id,
                IsActive = false,
                StaffType = StaffType.Teacher,
                TenantId = teacher.TenantId,
                UserId = teacher.Staff.UserId,
                Email = teacher.Staff.User.Email,
                FirstName = teacher.Staff.User.FirstName,
                LastName = teacher.Staff.User.LastName,
                Phone = teacher.Staff.User.PhoneNumber,
                ClassId = teacher.ClassId
            });

            result.Data = true;
            return result;
        }

    }
}
