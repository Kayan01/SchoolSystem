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
using IPagedList;
using Auth.Core.Interfaces.Setup;
using Auth.Core.Models.Setup;
using Auth.Core.Models.UserDetails;
using Shared.FileStorage;

namespace Auth.Core.Services
{
    public class StaffService : IStaffService
    {
        private readonly IRepository<Staff, long> _staffRepo;
        private readonly IRepository<TeachingStaff, long> _teachingStaffRepo;
        private readonly IRepository<Department, long> _departmentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IPublishService _publishService;
        private readonly IDocumentService _documentService;

        public StaffService(
            IRepository<Staff, long> staffRepo,
            IUnitOfWork unitOfWork,
            UserManager<User> userManagement,
            IRepository<TeachingStaff, long> teachingStaffRepo,
            IPublishService publishService,
            IRepository<Department,long> departmentRepo,
            IDocumentService documentService)
        {
            _staffRepo = staffRepo;
            _unitOfWork = unitOfWork;
            _userManager = userManagement;
            _teachingStaffRepo = teachingStaffRepo;
            _departmentRepo = departmentRepo;
            _publishService = publishService;
            _documentService = documentService;
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



            
            var pagedData = await query.ToPagedListAsync(model.PageIndex, model.PageSize);

            result.Data = new PaginatedModel<StaffVM>(pagedData.Select(x => x), model.PageIndex, model.PageSize, pagedData.TotalItemCount);

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

            //create staff
            var staff = _staffRepo.Insert(new Staff
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
                StaffType = StaffType.NonTeachingStaff,
                EmploymentDate = model.EmploymentDetails.EmploymentDate,
                ResumptionDate = model.EmploymentDetails.ResumptionDate,
                EmploymentStatus = model.EmploymentDetails.EmploymentStatus,
                DepartmentId = model.EmploymentDetails.DepartmentId,
                PayGrade = model.EmploymentDetails.PayGrade,
                HighestQualification = model.EmploymentDetails.HighestQualification,
                JobTitle = model.EmploymentDetails.JobTitle,
                NextOfKin = nextOfKin,
                WorkExperiences = workExperiences,
                EducationExperiences = eduExperiences,
                FileUploads = files

            });

            _staffRepo.Insert(staff);

            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.Commit();


            result.Data = new StaffVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

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