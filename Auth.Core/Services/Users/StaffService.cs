﻿using Shared.DataAccess.EfCore.UnitOfWork;
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
using Microsoft.OpenApi.Extensions;
using Shared.Tenancy;
using static Shared.Utils.CoreConstants;
using Shared.AspNetCore;
using Shared.Extensions;
using System.Data.SqlClient;
using System.Text;

namespace Auth.Core.Services
{
    public class StaffService : IStaffService
    {
        private readonly IRepository<Staff, long> _staffRepo;
        private readonly IRepository<Department, long> _departmentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IPublishService _publishService;
        private readonly IDocumentService _documentService;
        private readonly IHttpUserService _httpUserService;
        private readonly IAuthUserManagement _authUserManagement;
        private readonly ISchoolPropertyService _schoolPropertyService;

        public StaffService(
            IRepository<Staff, long> staffRepo,
            IUnitOfWork unitOfWork,
            UserManager<User> userManagement,
            IPublishService publishService,
            IRepository<Department,long> departmentRepo,
            IHttpUserService httpUserService,
            IDocumentService documentService,
            ISchoolPropertyService schoolPropertyService,
            IAuthUserManagement authUserManagement)
        {
            _staffRepo = staffRepo;
            _unitOfWork = unitOfWork;
            _userManager = userManagement;
            _departmentRepo = departmentRepo;
            _publishService = publishService;
            _documentService = documentService;
            _httpUserService = httpUserService;
            _schoolPropertyService = schoolPropertyService;
            _authUserManagement = authUserManagement;
        }

        public async Task<ResultModel<PaginatedModel<StaffVM>>> GetAllStaff(QueryModel model)
        {
            var result = new ResultModel<PaginatedModel<StaffVM>>();
            var query = _staffRepo.GetAll()
                .Where(x => x.StaffType == StaffType.NonTeachingStaff)
                .Select(x => new
                {
                    x.User.FirstName,
                    x.User.LastName,
                    x.User.Email,
                    x.Id,
                    x.StaffType,
                    x.User.PhoneNumber,
                    x.UserId,
                    x.RegNumber
                });



            
            var pagedData = await query.ToPagedListAsync(model.PageIndex, model.PageSize);

            result.Data = new PaginatedModel<StaffVM>(pagedData.Select(x => new StaffVM
            {
                StaffType = x.StaffType.GetDisplayName(),
                Email = x.Email,
                FirstName = x.FirstName,
                Id = x.Id,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                UserId = x.UserId,
                StaffNumber = x.RegNumber,
            }), model.PageIndex, model.PageSize, pagedData.TotalItemCount);

            return result;
        }

        public async Task<ResultModel<StaffDetailVM>> GetStaffById(long Id)
        {
            var result = new ResultModel<StaffDetailVM>();
            var staff = await _staffRepo.GetAll()
                            .Include(x => x.User)
                            .Include(x=> x.FileUploads)
                            .Include(x=> x.WorkExperiences)
                            .Include(x=> x.EducationExperiences)
                            .Include(x=> x.NextOfKin)
                            .Include(x=> x.Department)
                            .Where(x=> x.Id == Id && x.StaffType == StaffType.NonTeachingStaff)
                            .FirstOrDefaultAsync();


            result.Data = staff;
            return result;
        }

        public async Task<ResultModel<StaffVM>> AddStaff(AddStaffVM model)
        {
            var result = new ResultModel<StaffVM>();

            var schoolProperty = await _schoolPropertyService.GetSchoolProperty();
            if (schoolProperty.HasError)
            {
                result.AddError(schoolProperty.ValidationErrors);
                return result;
            }

            //check if department exist
            var dept = await _departmentRepo.GetAll().Where(x => x.Id == model.EmploymentDetails.DepartmentId).FirstOrDefaultAsync();

            if (dept == null)
            {
                result.AddError("Department does not exist");

                return result;
            }

            _unitOfWork.BeginTransaction();

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
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.UserType, StaffType.NonTeachingStaff.GetDescription()));

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
            var staff = new Staff
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
                Town = model.ContactDetails.Town,
                State = model.ContactDetails.State,
                Address = model.ContactDetails.Address,
                AltEmailAddress = model.ContactDetails.AltEmailAddress,
                AltPhoneNumber = model.ContactDetails.AltPhoneNumber,
                Country = model.ContactDetails.Country,
                NextOfKin = nextOfKin,
                WorkExperiences = workExperiences,
                EducationExperiences = eduExperiences,
                FileUploads = files

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
                    staff.RegNumber = $"{schoolProperty.Data.Prefix}{seperator}STF{seperator}{DateTime.Now.Year}{seperator}{nextNumber.ToString("00000")}";

                    _staffRepo.Insert(staff);
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


            result.Data = new StaffVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber, 
                StaffType = staff.StaffType.GetDisplayName(),
                StaffNumber = staff.RegNumber,
            };

            //broadcast login detail to email
            _ = await _authUserManagement.SendRegistrationEmail(user);


            //TODO Refactor, and Move teachers logic to TeachersService

            //Publish Message
            await _publishService.PublishMessage(Topics.Staff, BusMessageTypes.STAFF, new StaffSharedModel
                {
                    IsActive = true,
                    StaffType = staff.StaffType,
                    TenantId = staff.TenantId,
                    UserId = staff.UserId,
                    RegNumber = staff.RegNumber
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
                RegNumber = staff.RegNumber,
            });

            result.Data = staff;
            return result;
        }
    }
}