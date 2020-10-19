using Auth.Core.Models;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.Student;
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

namespace Auth.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IPublishService _publishService;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public StudentService(IRepository<Student, long> studentRepo,
            IUnitOfWork unitOfWork,
            IPublishService publishService,
            UserManager<User> userManager)
        {
            _studentRepo = studentRepo;
            _unitOfWork = unitOfWork;
            _publishService = publishService;
            _userManager = userManager;
        }

        public async Task<ResultModel<StudentVM>> AddStudentToSchool(CreateStudentVM model)
        {
            var result = new ResultModel<StudentVM>();

            _unitOfWork.BeginTransaction();

            //create auth user
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserType = UserType.Student,
            };

            var userResult = await _userManager.CreateAsync(user, model.Password);

            if (!userResult.Succeeded)
            {
                result.AddError(string.Join(';', userResult.Errors.Select(x => x.Description)));
                return result;
            }

            var stud = _studentRepo.Insert(new Student
            {
                UserId = user.Id
            });

            await _unitOfWork.SaveChangesAsync();

            //PublishMessage
            await _publishService.PublishMessage(Topics.Student, BusMessageTypes.STUDENT, new StudentSharedModel
            {
                Id = stud.Id,
                IsActive = true,
                ClassId = stud.ClassId,
                TenantId = stud.TenantId,
                UserId = stud.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.PhoneNumber
            });

            result.Data = new StudentVM
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Id = stud.Id
            };
            return result;
        }

        public async Task<ResultModel<bool>> DeleteStudent(long userId)
        {
            var result = new ResultModel<bool> { Data = false };

            //check if the student exists
            var std = await _studentRepo.FirstOrDefaultAsync(x => x.UserId == userId);

            if (std == null)
            {
                result.AddError("Student not found");
                return result;
            }

            //delete auth user

            await _unitOfWork.SaveChangesAsync();
            result.Data = true;

            return result;
        }

        public async Task<ResultModel<PaginatedModel<StudentVM>>> GetAllStudentsInSchool(QueryModel model)
        {

            var result = new ResultModel<PaginatedModel<StudentVM>>();

            var query = _studentRepo.GetAll()
                          .Include(x => x.Class)
                          .Include(x => x.User)
                          .Select(x => (StudentVM)x);

            var totalCount = query.Count();
            var pagedData = await PaginatedList<StudentVM>.CreateAsync(query, model.PageIndex, model.PageSize);

            result.Data = new PaginatedModel<StudentVM>(
                pagedData,
                model.PageIndex, 
                model.PageSize, totalCount
                ); 

            return result;
        }

        public async Task<ResultModel<StudentVM>> GetStudentById(long Id)
        {
            var result = new ResultModel<StudentVM>();
            var std = await _studentRepo.GetAll()
                .Include(x => x.Class)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == Id);

            if (std == null)
            {
                result.AddError("Student does not exist");
                return result;
            }

            result.Data = std;
            return result;
        }

        public async Task<ResultModel<StudentVM>> UpdateStudent(StudentUpdateVM model)
        {
            var result = new ResultModel<StudentVM>();

            var stud = await _studentRepo.GetAll()
                .Include(x => x.User)
                .Include(c => c.Class)
                .FirstOrDefaultAsync(x => x.UserId == model.UserId);

            if (stud == null)
            {
                result.AddError("Student does not exist");
                return result;
            }

            _unitOfWork.BeginTransaction();
            stud.ClassId = model.ClassId;

            var user = await _userManager.FindByIdAsync(model.UserId.ToString());

            if (user == null)
            {
                result.AddError("user not found");
                return result;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            //TODO: add more props

            await _studentRepo.UpdateAsync(stud);
            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            ////PublishMessage
            await _publishService.PublishMessage(Topics.Student, BusMessageTypes.STUDENT, new StudentSharedModel
            {
                IsActive = true,
                ClassId = stud.ClassId,
                TenantId = stud.TenantId,
                UserId = stud.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber
            });

            result.Data = stud;
            return result;
        }
    }
}