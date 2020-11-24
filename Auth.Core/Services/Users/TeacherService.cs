using Auth.Core.Context;
using Auth.Core.Interfaces.Users;
using Auth.Core.Models;
using Auth.Core.Models.Setup;
using Auth.Core.Models.UserDetails;
using Auth.Core.Models.Users;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.Staff;
using IPagedList;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Entities;
using Shared.Enums;
using Shared.Extensions;
using Shared.FileStorage;
using Shared.Pagination;
using Shared.PubSub;
using Shared.Utils;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using static Shared.Utils.CoreConstants;

namespace Auth.Core.Services.Users
{
    public class TeacherService : ITeacherService
    {
        private readonly IRepository<TeachingStaff, long> _teacherRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IDocumentService _documentService;
        private readonly IRepository<Department, long> _departmentRepo;
        private readonly IPublishService _publishService;
        private readonly IAuthUserManagement _authUserManagement;
        private readonly ILogger<TeacherService> _logger;

        public TeacherService(UserManager<User> userManager,
            IRepository<TeachingStaff, long> teacherRepo,
            IUnitOfWork unitOfWork,
            IDocumentService documentService,
            IRepository<Department, long> departmentRepo,
            IAuthUserManagement authUserManagement,
            ILogger<TeacherService> logger,
            IPublishService publishService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _teacherRepo = teacherRepo;
            _publishService = publishService;
            _authUserManagement = authUserManagement;
            _logger = logger;
            _departmentRepo = departmentRepo;
            _documentService = documentService;
        }

        public async Task<ResultModel<PaginatedModel<TeacherVM>>> GetTeachers(QueryModel model)
        {
            var result = new ResultModel<PaginatedModel<TeacherVM>>();
            var query = _teacherRepo.GetAll()
                          .Select(x => new
                          {
                              x.Id,
                              x.Staff.User.Email,
                              x.Staff.User.LastName,
                              x.Staff.User.PhoneNumber, 
                              x.Staff.User.FirstName,
                               x.Staff.StaffType
                          }
                          );

            var pagedData = await query.ToPagedListAsync(model.PageIndex, model.PageSize);



            result.Data = new PaginatedModel<TeacherVM>(pagedData.Select(x => new TeacherVM
            {
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                LastName = x.LastName,
                Id = x.Id,
                FirstName = x.FirstName,
                StaffType = x.StaffType.GetDescription()
            }), model.PageIndex, model.PageSize, pagedData.TotalItemCount);

            return result;            
        }

        public async Task<ResultModel<TeacherVM>> GetTeacherById(long Id)
        {
            var result = new ResultModel<TeacherVM>();
            var query = _teacherRepo.GetAll()
                            .Include(x => x.Staff.User)
                            .Include(x => x.Class)
                            .Where(x => x.Id == Id)
                            .FirstOrDefault();

            result.Data = query;
            return result ;
        }

        public async Task<ResultModel<TeacherVM>> AddTeacher(AddTeacherVM model)
        {
            var result = new ResultModel<TeacherVM>();

            _unitOfWork.BeginTransaction();

            //check if department exist
            var dept = await _departmentRepo.GetAll().Where(x => x.Id == model.EmploymentDetails.DepartmentId).FirstOrDefaultAsync();

            if (dept == null)
            {
                result.AddError("Department does not exist");

                return result;
            }

            //save filles
            var files = new List<FileUpload>();

            if (model.Files != null && model.Files.Any())
            {
                if (model.Files.Count != model.DocumentTypes.Count)
                {
                    result.AddError("Some document types are missing");
                    return result;
                }
                files = await _documentService.TryUploadSupportingDocuments(model.Files, model.DocumentTypes);
                if (files.Count() != model.Files.Count())
                {
                    result.AddError("Some files could not be uploaded");

                    return result;
                }
            }


            //create auth user
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.ContactDetails.EmailAddress,
                UserName = model.ContactDetails.EmailAddress,
                PhoneNumber = model.ContactDetails.PhoneNumber,
                MiddleName = model.OtherNames,
                UserType = UserType.Staff,
            };

            var userResult = await _userManager.CreateAsync(user,model.ContactDetails.PhoneNumber);

            if (!userResult.Succeeded)
            {
                result.AddError(string.Join(';', userResult.Errors.Select(x => x.Description)));
                return result;
            }

            //create next of kin
            var nextOfKin = new NextOfKin
            {
                Address = model.NextOfKin.NextKinAddress,
                Country = model.NextOfKin.NextKinCountry,
                FirstName = model.NextOfKin.NextKinFirstName,
                LastName = model.NextOfKin.NextKinLastName,
                Occupation = model.NextOfKin.NextKinOccupation,
                OtherName = model.NextOfKin.NextKinOtherName,
                Phone = model.NextOfKin.NextKinPhone,
                Relationship = model.NextOfKin.NextKinRelationship,
                State = model.NextOfKin.NextKinState,
                Town = model.NextOfKin.NextKinTown
            };

            //get all workexperiences
            var workExperiences = new List<WorkExperience>();
            foreach (var wk in model.WorkExperienceVMs)
            {
                workExperiences.Add(new WorkExperience
                {
                    EndTime = wk.EndTime,
                    StartTime = wk.StartTime,
                    WorkCompanyName = wk.WorkCompanyName,
                    WorkRole = wk.WorkRole
                });
            }
            
            //get all education experience
            var eduExperiences = new List<EducationExperience>();
            foreach (var edu in model.EducationExperienceVMs)
            {
                eduExperiences.Add(new EducationExperience
                {
                    EducationSchoolName = edu.EducationSchoolName,
                    EducationQualification = edu.EducationSchoolQualification,
                    StartDate = edu.StartDate,
                    EndDate = edu.EndDate
                });
            }

            var teacher = _teacherRepo.Insert(new TeachingStaff
            {
                
                Staff = new Staff
                {

                    UserId = user.Id,
                    BloodGroup = model.BloodGroup,
                    DateOfBirth = model.DateOfBirth,
                    IsActive = model.IsActive,
                    LocalGovernment = model.LocalGovernment,
                    MaritalStatus = model.MaritalStatus,
                    Nationality = model.Nationality,
                    Religion = model.Religion,
                    StateOfOrigin = model.StateOfOrigin,
                    Sex = model.Sex,
                    StaffType = StaffType.TeachingStaff,
                    EmploymentDate = model.EmploymentDetails.EmploymentDate,
                    ResumptionDate = model.EmploymentDetails.ResumptionDate,
                    EmploymentStatus = model.EmploymentDetails.EmploymentStatus,
                    DepartmentId = model.EmploymentDetails.DepartmentId,
                    PayGrade = model.EmploymentDetails.PayGrade,
                    HighestQualification = model.EmploymentDetails.HighestQualification,
                    Town = model.ContactDetails.Town,
                    State = model.ContactDetails.State,
                    Address = model.Address,
                    AltEmailAddress = model.AltEmailAddress,
                    AltPhoneNumber = model.AltPhoneNumber,
                    Country = model.Country,
                    JobTitle = model.EmploymentDetails.JobTitle,
                    NextOfKin = nextOfKin,
                    WorkExperiences = workExperiences,
                    EducationExperiences = eduExperiences,
                    FileUploads = files

                }
            });

           teacher.Staff.TenantId = teacher.TenantId;//TODO remove this when the tenant Id is automatically added to Staff


            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.Commit();

            await _publishService.PublishMessage(Topics.Teacher, BusMessageTypes.TEACHER, new TeacherSharedModel
            {
                Id = teacher.Id,
                IsActive = true,
                StaffType = StaffType.TeachingStaff,
                TenantId = teacher.TenantId,
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber
            });

            //Email and Notifications
            var notificationResult = await NewTeacherNotification(teacher, user.Email);

            if (notificationResult.HasError)
                _logger.LogError($"Failed to send notifications for: {teacher.Staff.User.FullName} - {teacher.Staff.User.Email}, Reason: {string.Join(';', notificationResult.ErrorMessages)}");

            result.Data = new TeacherVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
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
                StaffType = StaffType.TeachingStaff,
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
                StaffType = StaffType.TeachingStaff,
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

        #region notification

        private async Task<ResultModel<bool>> NewTeacherNotification(TeachingStaff teacher, string email)
        {
            var result = await _authUserManagement.GetPasswordRestCode(email);

            var admins = new long[] { 1 };//TODO Get all admin user Ids

            if (result.HasError)
                return new ResultModel<bool>(result.ErrorMessages);

            await _publishService.PublishMessage(Topics.Notification, BusMessageTypes.NOTIFICATION, new CreateNotificationModel
            {
                Emails = new List<CreateEmailModel>
                {
                    new CreateEmailModel(EmailTemplateType.NewUser, new Dictionary<string, string>{ { "Code", result.Data.code} }, result.Data.user),
                    new CreateEmailModel(EmailTemplateType.NewTeacher, new Dictionary<string, string>{ }, result.Data.user)
                },
                Notifications = new List<InAppNotificationModel>
                {
                    new InAppNotificationModel("Welcome new teacher to Sch-Track", EntityType.Teacher, result.Data.user.Id, new[] { result.Data.user.Id }.ToList()),
                    new InAppNotificationModel("A new teacher has been added", EntityType.Teacher, result.Data.user.Id, admins.ToList()),
                }
            });

            return new ResultModel<bool>(true, "Success");
        }

        #endregion

    }
}
