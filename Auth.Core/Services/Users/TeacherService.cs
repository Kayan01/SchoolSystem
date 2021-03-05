using Auth.Core.Context;
using Auth.Core.Interfaces.Setup;
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
using Shared.AspNetCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Entities;
using Shared.Enums;
using Shared.Extensions;
using Shared.FileStorage;
using Shared.Pagination;
using Shared.PubSub;
using Shared.Tenancy;
using Shared.Utils;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Shared.Utils.CoreConstants;

namespace Auth.Core.Services.Users
{
    public class TeacherService : ITeacherService
    {
        private readonly IRepository<TeachingStaff, long> _teacherRepo;
        private readonly IRepository<Staff, long> _staffRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IDocumentService _documentService;
        private readonly IRepository<Department, long> _departmentRepo;
        private readonly IPublishService _publishService;
        private readonly IAuthUserManagement _authUserManagement;
        private readonly ILogger<TeacherService> _logger;
        private readonly IStaffService _staffService;
        private readonly IHttpUserService _httpUserService;
        private readonly ISchoolPropertyService _schoolPropertyService;

        public TeacherService(UserManager<User> userManager,
            IRepository<TeachingStaff, long> teacherRepo,
            IRepository<Staff, long> staffRepo,
            IUnitOfWork unitOfWork,
            IDocumentService documentService,
            IRepository<Department, long> departmentRepo,
            IAuthUserManagement authUserManagement,
            ILogger<TeacherService> logger,
            IPublishService publishService,
            IHttpUserService httpUserService,
            IStaffService staffService,
            ISchoolPropertyService schoolPropertyService)
        {
            _userManager = userManager;
            _staffRepo = staffRepo;
            _unitOfWork = unitOfWork;
            _teacherRepo = teacherRepo;
            _publishService = publishService;
            _authUserManagement = authUserManagement;
            _logger = logger;
            _departmentRepo = departmentRepo;
            _documentService = documentService;
            _staffService = staffService;
            _httpUserService = httpUserService;
            _schoolPropertyService = schoolPropertyService;
        }

        public async Task<ResultModel<PaginatedModel<TeacherVM>>> GetTeachers(QueryModel model)
        {
            var result = new ResultModel<PaginatedModel<TeacherVM>>();
            var query = _teacherRepo.GetAll()
                          .Select(x => new
                          {
                              x.Id,
                              x.Staff.UserId,
                              x.Staff.User.Email,
                              x.Staff.User.LastName,
                              x.Staff.User.PhoneNumber,
                              x.Staff.User.FirstName,
                              x.Staff.StaffType,
                              x.Staff.RegNumber
                          }
                          );

            var pagedData = await query.ToPagedListAsync(model.PageIndex, model.PageSize);



            result.Data = new PaginatedModel<TeacherVM>(pagedData.Select(x => new TeacherVM
            {
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                LastName = x.LastName,
                Id = x.Id,
                UserId = x.UserId,
                FirstName = x.FirstName,
                StaffType = x.StaffType.GetDescription(),
                StaffNumber = x.RegNumber,
            }), model.PageIndex, model.PageSize, pagedData.TotalItemCount);

            return result;
        }

        public async Task<ResultModel<TeacherDetailVM>> GetTeacherById(long Id)
        {
            var result = new ResultModel<TeacherDetailVM>();
            var query = _teacherRepo.GetAll()
                            .Where(x => x.Id == Id)
                           .Include(x => x.Staff)
                           .ThenInclude(m => m.User)
                           .Include(x => x.Staff)
                           .ThenInclude(x => x.FileUploads)
                           .Include(x => x.Class)
                           .Include(x => x.Staff)
                           .ThenInclude(x => x.WorkExperiences)
                           .Include(x => x.Staff)
                           .ThenInclude(x => x.NextOfKin)
                           .Include(x => x.Staff)
                           .ThenInclude(x => x.EducationExperiences)
                            .Include(x => x.Class)
                            .FirstOrDefault();

            result.Data = query;
            return result;
        }

        public async Task<ResultModel<TeacherVM>> AddTeacher(AddStaffVM model)
        {
            var result = new ResultModel<TeacherVM>();

            var schoolProperty = await _schoolPropertyService.GetSchoolProperty();
            if (schoolProperty.HasError)
            {
                result.AddError(schoolProperty.ValidationErrors);
                return result;
            }

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

            var userResult = await _userManager.CreateAsync(user, model.ContactDetails.PhoneNumber);

            if (!userResult.Succeeded)
            {
                result.AddError(string.Join(';', userResult.Errors.Select(x => x.Description)));
                return result;
            }

            //Add TenantId to UserClaims
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.TenantId, _httpUserService.GetCurrentUser().TenantId?.ToString()));
            //add stafftype to claims
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.UserType, StaffType.TeachingStaff.GetDescription()));

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

            var teacher = new TeachingStaff
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
                    Address = model.ContactDetails.Address,
                    AltEmailAddress = model.ContactDetails.AltEmailAddress,
                    AltPhoneNumber = model.ContactDetails.AltPhoneNumber,
                    Country = model.ContactDetails.Country,
                    JobTitle = model.EmploymentDetails.JobTitle,
                    NextOfKin = nextOfKin,
                    WorkExperiences = workExperiences,
                    EducationExperiences = eduExperiences,
                    FileUploads = files

                }
            };

            var lastRegNumber = await _staffRepo.GetAll().OrderBy(m => m.Id).Select(m => m.RegNumber).LastAsync();
            var lastNumber = 0;
            var seperator = schoolProperty.Data.Seperator;
            if (!string.IsNullOrWhiteSpace(lastRegNumber))
            {
                lastNumber = int.Parse(lastRegNumber.Split(seperator).Last());
            }
            var nextNumber = lastNumber;

            var saved = false;

            while (!saved)
            {
                try
                {
                    nextNumber++;
                    teacher.Staff.RegNumber = $"{schoolProperty.Data.Prefix}{seperator}STF{seperator}{DateTime.Now.Year}{seperator}{nextNumber.ToString("00000")}";

                    _teacherRepo.Insert(teacher);


                    teacher.Staff.TenantId = teacher.TenantId;//TODO remove this when the tenant Id is automatically added to Staff
                    await _unitOfWork.SaveChangesAsync();

                    saved = true;
                }
                // 2627 is unique constraint (includes primary key), 2601 is unique index
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException && (sqlException.Number == 2627 || sqlException.Number == 2601))
                {
                    saved = false;
                }
            }

            _unitOfWork.Commit();


            //broadcast login detail to email
            _ = await _authUserManagement.SendRegistrationEmail(user);

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
                Phone = user.PhoneNumber,
                RegNumber = teacher.Staff.RegNumber,
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

        public async Task<ResultModel<TeacherVM>> UpdateTeacher(UpdateTeacherVM model, long Id)
        {
            var result = new ResultModel<TeacherVM>();

            var teacher = _teacherRepo.GetAll()
                           .Where(x => x.Id == Id)
                           .Include(x => x.Staff)
                           .ThenInclude(m => m.User)
                           .Include(x => x.Staff)
                           .ThenInclude(x => x.FileUploads)
                           .Include(x => x.Class)
                           .Include(x=> x.Staff)
                           .ThenInclude(x=> x.WorkExperiences)
                           .Include(x=> x.Staff)
                           .ThenInclude(x=> x.NextOfKin)
                           .Include(x=> x.Staff)
                           .ThenInclude(x=> x.EducationExperiences)
                           .FirstOrDefault();

            if (teacher == null)
            {
                result.AddError($"Teacher not found");
                return result;
            }


            var schoolProperty = await _schoolPropertyService.GetSchoolProperty();
            if (schoolProperty.HasError)
            {
                _ = schoolProperty.ErrorMessages.Select(x => { result.AddError(x); return x; });
                return result;
            }

            _unitOfWork.BeginTransaction();

            //check if department exist
            var dept = await _departmentRepo.GetAll().
                Where(x => x.Id == model.EmploymentDetails.DepartmentId).FirstOrDefaultAsync();

            if (dept == null)
            {
                result.AddError("Department does not exist");

                return result;
            }

            //save files
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

            //update auth user
            teacher.Staff.User.FirstName = model.FirstName;
            teacher.Staff.User.LastName = model.LastName;
            teacher.Staff.User.Email = model.ContactDetails.EmailAddress;
            teacher.Staff.User.UserName = model.ContactDetails.EmailAddress;
            teacher.Staff.User.PhoneNumber = model.ContactDetails.PhoneNumber;
            teacher.Staff.User.MiddleName = model.OtherNames;
            teacher.Staff.User.UserType = UserType.Staff;


            var userResult = await _userManager.UpdateAsync(teacher.Staff.User);

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



            teacher.Staff.BloodGroup = model.BloodGroup;
            teacher.Staff.DateOfBirth = model.DateOfBirth;
            teacher.Staff.IsActive = model.IsActive;
            teacher.Staff.LocalGovernment = model.LocalGovernment;
            teacher.Staff.MaritalStatus = model.MaritalStatus;
            teacher.Staff.Nationality = model.Nationality;
            teacher.Staff.Religion = model.Religion;
            teacher.Staff.StateOfOrigin = model.StateOfOrigin;
            teacher.Staff.Sex = model.Sex;
            teacher.Staff.StaffType = StaffType.TeachingStaff;
            teacher.Staff.EmploymentDate = model.EmploymentDetails.EmploymentDate;
            teacher.Staff.ResumptionDate = model.EmploymentDetails.ResumptionDate;
            teacher.Staff.EmploymentStatus = model.EmploymentDetails.EmploymentStatus;
            teacher.Staff.DepartmentId = model.EmploymentDetails.DepartmentId;
            teacher.Staff.PayGrade = model.EmploymentDetails.PayGrade;
            teacher.Staff.HighestQualification = model.EmploymentDetails.HighestQualification;
            teacher.Staff.Town = model.ContactDetails.Town;
            teacher.Staff.State = model.ContactDetails.State;
            teacher.Staff.Address = model.ContactDetails.Address;
            teacher.Staff.AltEmailAddress = model.ContactDetails.AltEmailAddress;
            teacher.Staff.AltPhoneNumber = model.ContactDetails.AltPhoneNumber;
            teacher.Staff.Country = model.ContactDetails.Country;
            teacher.Staff.JobTitle = model.EmploymentDetails.JobTitle;
            teacher.Staff.NextOfKin = nextOfKin;
            teacher.Staff.WorkExperiences = workExperiences;
            teacher.Staff.EducationExperiences = eduExperiences;

            if (model.Files != null)
            {
                teacher.Staff.FileUploads = files;
            }

            await _teacherRepo.UpdateAsync(teacher);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();

            await _publishService.PublishMessage(Topics.Teacher, BusMessageTypes.TEACHER_UPDATE, new TeacherSharedModel
            {
                Id = teacher.Id,
                IsActive = true,
                StaffType = StaffType.TeachingStaff,
                TenantId = teacher.TenantId,
                UserId = teacher.Staff.User.Id,
                Email = teacher.Staff.User.Email,
                FirstName = teacher.Staff.User.FirstName,
                LastName = teacher.Staff.User.LastName,
                Phone = teacher.Staff.User.PhoneNumber,
                ClassId = teacher.ClassId,
                RegNumber = teacher.Staff.RegNumber
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

        public async Task<ResultModel<string>> MakeClassTeacher(ClassTeacherVM model)
        {
            var result = new ResultModel<string>();

            var teacher = await _teacherRepo.GetAll().Where(x => x.Id == model.TeacherId)
                            .Include(x => x.Staff).ThenInclude(m => m.User)
                            .FirstOrDefaultAsync();

            if (teacher == null)
            {
                result.AddError($"Teacher not found");
                return result;
            }

            teacher.ClassId = model.ClassId;

            await _teacherRepo.UpdateAsync(teacher);
            _unitOfWork.SaveChanges();

            //adds classID as a claim
            await _userManager.AddClaimAsync(teacher.Staff.User, new System.Security.Claims.Claim(ClaimsKey.TeacherClassId, model.ClassId.ToString()));

            await _publishService.PublishMessage(Topics.Teacher, BusMessageTypes.TEACHER, new TeacherSharedModel
            {
                Id = teacher.Id,
                IsActive = teacher.Staff.IsActive,
                StaffType = teacher.Staff.StaffType,
                TenantId = teacher.TenantId,
                UserId = teacher.Staff.User.Id,
                Email = teacher.Staff.User.Email,
                FirstName = teacher.Staff.User.FirstName,
                LastName = teacher.Staff.User.LastName,
                Phone = teacher.Staff.User.PhoneNumber
            });

            result.Data = "Saved";
            return result;
        }

        public async Task<ResultModel<ClassTeacherVM>> GetTeacherClassById(long Id)
        {
            var result = new ResultModel<ClassTeacherVM>();
            var query = await _teacherRepo.GetAll()
                            .Where(x => x.Id == Id)
                            .Select(m => new ClassTeacherVM()
                            {
                                ClassId = m.Class.Id,
                                TeacherId = m.Id,
                                ClassName = $"{m.Class.Name} {m.Class.ClassArm}",
                                ClassSection = m.Class.SchoolSection.Name,
                            }
                                )
                            .FirstOrDefaultAsync();

            result.Data = query;
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
                    new CreateEmailModel(EmailTemplateType.NewUser, new Dictionary<string, string>{
                        { "Code", result.Data.code}
                    }, result.Data.user),
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
