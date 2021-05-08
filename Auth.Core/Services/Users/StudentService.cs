using Auth.Core.Interfaces.Setup;
using Auth.Core.Models;
using Auth.Core.Models.Medical;
using Auth.Core.Models.Users;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.Student;
using IPagedList;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
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
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Utils.CoreConstants;

namespace Auth.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IPublishService _publishService;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IRepository<Parent, long> _parentRepo;
        private readonly IRepository<SchoolClass, long> _classRepo;
        private readonly IRepository<School, long> _schoolRepo;
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IHttpUserService _httpUserService;
        private readonly ISchoolPropertyService _schoolPropertyService;

        private readonly IAuthUserManagement _authUserManagement;
        public StudentService(
            IRepository<Student, long> studentRepo,
            IRepository<Parent, long> parentRepo,
            IRepository<SchoolClass, long> classRepo,
            IRepository<School, long> schoolRepo,
            IDocumentService documentService,
            IUnitOfWork unitOfWork,
            IPublishService publishService,
            IHttpUserService httpUserService,
            UserManager<User> userManager,
            ISchoolPropertyService schoolPropertyService,
            IAuthUserManagement authUserManagement)
        {
            _studentRepo = studentRepo;
            _classRepo = classRepo;
            _parentRepo = parentRepo;
            _schoolRepo = schoolRepo;
            _unitOfWork = unitOfWork;
            _documentService = documentService;
            _publishService = publishService;
            _userManager = userManager;
            _httpUserService = httpUserService;
            _schoolPropertyService = schoolPropertyService;
            _authUserManagement = authUserManagement;
        }

        public async Task<ResultModel<StudentVM>> AddStudentToSchool(CreateStudentVM model)
        {
            var result = new ResultModel<StudentVM>();

            var schoolProperty = await _schoolPropertyService.GetSchoolProperty();
            if (schoolProperty.HasError)
            {
                result.AddError(schoolProperty.ValidationErrors);
                return result;
            }

            _unitOfWork.BeginTransaction();

            //check if parent exists
            var parent = await _parentRepo.GetAll().Include(m=>m.User)
                .Where(x => x.Id == model.ParentId)
                .FirstOrDefaultAsync();

            if (parent == null)
            {
                result.AddError("No parent exists");
                return result;
            }

            //check if class exists
            var @class = await _classRepo.GetAll().Where(x => x.Id == model.ClassId).FirstOrDefaultAsync();
            if (@class == null)
            {
                result.AddError("class does not exists");
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
                Email = model.ContactEmail.Trim(),
                UserName = model.ContactEmail.Trim(),
                PhoneNumber = model.ContactPhone,
                UserType = UserType.Student,
            };

            var userResult = await _userManager.CreateAsync(user, model.ContactPhone);

            if (!userResult.Succeeded)
            {
                result.AddError(string.Join(';', userResult.Errors.Select(x => x.Description)));
                return result;
            }

            //Add TenantId to UserClaims
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.TenantId, _httpUserService.GetCurrentUser().TenantId?.ToString()));
            //add stafftype to claims
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.UserType, UserType.Student.GetDescription()));
            
            var medicalHistory = new MedicalDetail {
                Allergies = model.Allergies,
                BloodGroup = model.BloodGroup,
                ConfidentialNotes = model.ConfidentialNotes,
                Disability = model.Disability,
                Genotype = model.Genotype
            };
            var immunizations = new List<ImmunizationHistory>();

            foreach (var im in model.ImmunizationVms)
            {
                immunizations.Add(new ImmunizationHistory
                {
                    Age = im.Age,
                    DateImmunized = im.DateImmunized,
                    Vaccine = im.Vaccine
                });
            }

            medicalHistory.ImmunizationHistories = immunizations;

            var stud = new Student
            {
                UserId = user.Id,
                Address = model.ContactAddress,
                AdmissionDate = model.AdmissionDate,
                ClassId = model.ClassId,
                Country = model.ContactCountry,
                DateOfBirth = model.DateOfBirth,
                EntryType = model.EntryType,
                FileUploads = files,
                Level = model.Level,
                LocalGovernment = model.LocalGovt,
                MedicalDetail = medicalHistory,
                MothersMaidenName = model.MothersMaidenName,
                Nationality = model.Nationality,
                ParentId = model.ParentId,
                TransportRoute = model.TransportRoute,
                Religion = model.Religion,
                Sex = model.Sex,
                State = model.ContactState,
                StateOfOrigin = model.StateOfOrigin,
                StudentType = model.StudentType,
                Town = model.ContactTown,
                 IsActive = true
            };

            var lastRegNumber = await _studentRepo.GetAll().OrderBy(m => m.Id).Select(m => m.RegNumber).LastOrDefaultAsync();
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
                    stud.RegNumber = $"{schoolProperty.Data.Prefix}{seperator}STT{seperator}{DateTime.Now.Year}{seperator}{nextNumber:00000}";

                    _studentRepo.Insert(stud);
                    await _unitOfWork.SaveChangesAsync();

                    saved = true;
                }
                // 2627 is unique constraint (includes primary key), 2601 is unique index
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException && (sqlException.Number == 2627 || sqlException.Number == 2601))
                {
                    saved = false;
                }
            }


            //change user's username to reg number
            user.UserName = stud.RegNumber;
            user.NormalizedUserName = stud.RegNumber.ToUpper();
            await _userManager.UpdateAsync(user);

            //add classId to claims
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.StudentClassId, stud.ClassId.ToString()));

            _unitOfWork.Commit();

            var school = await _schoolRepo.GetAll().Where(m => m.Id == stud.TenantId).FirstOrDefaultAsync();
            //broadcast login detail to email
            _ = await _authUserManagement.SendRegistrationEmail(user, school.DomainName);

            //PublishMessage
            await _publishService.PublishMessage(Topics.Student, BusMessageTypes.STUDENT, new StudentSharedModel
            {
                Id = stud.Id,
                RegNumber = stud.RegNumber,
                IsActive = true,
                ClassId = stud.ClassId,
                TenantId = stud.TenantId,
                UserId = stud.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.ContactEmail,
                Phone = model.ContactPhone,
                ParentName = $"{parent.User.FirstName} {parent.User.LastName}",
                ParentEmail = parent.User.Email,
                ParentId = parent.Id,
                Sex = model.Sex,
                DoB = model.DateOfBirth,
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
            var std = await _studentRepo.FirstOrDefaultAsync(x => x.Id == userId);

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
                .OrderByDescending(x=> x.CreationTime)
                .Select(x => new StudentVM
                {
                     Id = x.Id,
                    Class = x.Class.FullName,
                    DateOfBirth = x.DateOfBirth,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    StudentNumber = x.RegNumber,
                    Sex = x.Sex,
                    Section = x.Class.SchoolSection.Name,
                    IsActive = x.IsActive,
                    ImagePath = x.FileUploads.Where(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName()).Select(x => x.Path).FirstOrDefault()
                });

            var pagedData = await query.ToPagedListAsync(model.PageIndex, model.PageSize);
            
            result.Data = new PaginatedModel<StudentVM>(pagedData, model.PageIndex, model.PageSize, pagedData.TotalItemCount);

            return result;
        }

        public async Task<ResultModel<PaginatedModel<StudentVM>>> GetAllStudentsInClass(QueryModel model, long classId)
        {
            var query = _studentRepo.GetAll()
                .Where(x => x.ClassId == classId)
                .Select(x=> new StudentVM
            {
                Id = x.Id,
                Class = x.Class.FullName,
                DateOfBirth = x.DateOfBirth,
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                StudentNumber = x.RegNumber,
                Sex = x.Sex,
                Email = x.User.Email,
                PhoneNumber =  x.User.PhoneNumber  ,
                Section = x.Class.SchoolSection.Name,
                IsActive = x.IsActive,
                ImagePath = x.FileUploads.Where(fileUpload => fileUpload.Name == DocumentType.ProfilePhoto.GetDisplayName()).Select(x => x.Path).FirstOrDefault()
            });

            var pagedData = await query.ToPagedListAsync(model.PageIndex, model.PageSize);
           

            return new ResultModel<PaginatedModel<StudentVM>>(data: new PaginatedModel<StudentVM>(pagedData, model.PageIndex, model.PageSize, pagedData.TotalItemCount));
        }

        public async Task<ResultModel<StudentDetailVM>> GetStudentById(long Id)
        {
            var result = new ResultModel<StudentDetailVM>();
            var std = await _studentRepo.GetAll().Where(x => x.Id == Id)
                        .Select(x => new
                        {
                            x.Id,
                            x.User.FirstName,
                            x.User.LastName,
                            x.MothersMaidenName,
                            x.Sex,
                            x.RegNumber,
                            x.DateOfBirth,
                            ParentName =  x.Parent.User.FullName,
                            x.ParentId,
                            x.Nationality,
                            x.Religion,
                            x.LocalGovernment,
                            x.StateOfOrigin,
                            x.EntryType,
                            x.AdmissionDate,
                            x.Level,
                            ClassName = x.Class.FullName,
                            SchoolSection =  x.Class.SchoolSection.Name,
                            x.StudentType,
                            x.MedicalDetail.BloodGroup,
                            x.MedicalDetail.Genotype,
                            x.MedicalDetail.Allergies,
                            x.MedicalDetail.ConfidentialNotes,
                            Immunization = x.MedicalDetail.ImmunizationHistories,
                            x.User.PhoneNumber,
                            x.User.Email,
                            x.Country,
                            x.Address,
                            x.Town,
                            x.State,
                            x.IsActive,
                            image = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName())
                        }).FirstOrDefaultAsync();

            if (std == null)
            {
                result.AddError("Student does not exist");
                return result;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in std.Immunization)
            {
                sb.AppendLine($"{item.DateImmunized} {item.Vaccine}");
            }
            result.Data = new StudentDetailVM
            {
                StudentType = std.StudentType.GetDisplayName(),
                StateOfOrigin = std.StateOfOrigin,
                State = std.State,
                Sex = std.Sex,
                RegNumber = std.RegNumber,
                Section = std.SchoolSection,
                AdmissionDate = std.AdmissionDate,
                Allergies = std.Allergies,
                BloodGroup = std.BloodGroup,
                Class = std.ClassName,
                City = std.Town,
                ConfidentialNote = std.ConfidentialNotes,
                DateOfBirth = std.DateOfBirth,
                Country = std.Country,
                EmailAddress = std.Email,
                FirstName = std.FirstName,
                Genotype = std.Genotype,
                HomeAddress = std.Address,
                Id = std.Id,
                ParentId = std.ParentId,
                Image = _documentService.TryGetUploadedFile(std.image?.Path),
                ImmunizationHistoryVMs = std.Immunization.Select(x=> new ImmunizationHistoryVM { Age = x.Age, DateImmunized = x.DateImmunized, Vaccine = x.Vaccine}).ToList(),
                LastName = std.LastName,
                LocalGovernment = std.LocalGovernment,
                MothersMaidenName = std.MothersMaidenName,
                Nationality = std.Nationality,
                ParentName = std.ParentName,
                PhoneNumber = std.PhoneNumber,
                Religion = std.Religion,
                IsActive = std.IsActive

            };
            return result;
        }

        public async Task<ResultModel<StudentDetailVM>> GetStudentProfileById(long Id)
        {
            var result = new ResultModel<StudentDetailVM>();
            var std = await _studentRepo.GetAll().Where(x => x.UserId == Id)
                        .Select(x => new
                        {
                            x.Id,
                            x.RegNumber,
                            x.User.FirstName,
                            x.User.LastName,
                            x.MothersMaidenName,
                            x.Sex,
                            x.DateOfBirth,
                            ParentName = x.Parent.User.FullName,
                            x.Nationality,
                            x.Religion,
                            x.LocalGovernment,
                            x.StateOfOrigin,
                            x.EntryType,
                            x.AdmissionDate,
                            x.Level,
                            ClassName = x.Class.FullName,
                            SchoolSection = x.Class.SchoolSection.Name,
                            x.StudentType,
                            x.MedicalDetail.BloodGroup,
                            x.MedicalDetail.Genotype,
                            x.MedicalDetail.Allergies,
                            x.MedicalDetail.ConfidentialNotes,
                            Immunization = x.MedicalDetail.ImmunizationHistories,
                            x.User.PhoneNumber,
                            x.User.Email,
                            x.Country,
                            x.Address,
                            x.Town,
                            x.State,
                            x.IsActive,
                            image = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName())
                        }).FirstOrDefaultAsync();

            if (std == null)
            {
                result.AddError("Student does not exist");
                return result;
            }

           
            result.Data = new StudentDetailVM
            {
                StudentType = std.StudentType.GetDisplayName(),
                StateOfOrigin = std.StateOfOrigin,
                State = std.State,
                Sex = std.Sex,
                Section = std.SchoolSection,
                AdmissionDate = std.AdmissionDate,
                Allergies = std.Allergies,
                BloodGroup = std.BloodGroup,
                Class = std.ClassName,
                City = std.Town,
                ConfidentialNote = std.ConfidentialNotes,
                DateOfBirth = std.DateOfBirth,
                Country = std.Country,
                EmailAddress = std.Email,
                FirstName = std.FirstName,
                Genotype = std.Genotype,
                HomeAddress = std.Address,
                Id = std.Id,
                Image = _documentService.TryGetUploadedFile(std.image?.Path),
                ImmunizationHistoryVMs = std.Immunization.Select(x => new ImmunizationHistoryVM { Age = x.Age, DateImmunized = x.DateImmunized, Vaccine = x.Vaccine }).ToList(),
                LastName = std.LastName,
                LocalGovernment = std.LocalGovernment,
                MothersMaidenName = std.MothersMaidenName,
                Nationality = std.Nationality,
                ParentName = std.ParentName,
                PhoneNumber = std.PhoneNumber,
                Religion = std.Religion,
                RegNumber = std.RegNumber,
                IsActive = std.IsActive
            };
            return result;
        }

        public async Task<ResultModel<StudentVM>> UpdateStudent(long Id, StudentUpdateVM model)
        {
            var result = new ResultModel<StudentVM>();

            var stud = await _studentRepo.GetAll()
                .Where(x => x.Id == Id)
                .Include(x => x.User)
                .Include(c => c.Class)
                .FirstOrDefaultAsync();

            if (stud == null)
            {
                result.AddError("Student does not exist");
                return result;
            }

            //check if parent exists
            var parent = await _parentRepo.GetAll().Include(m=>m.User)
                .Where(x => x.Id == model.ParentId)
                .FirstOrDefaultAsync();

            if (parent == null)
            {
                result.AddError("No parent exists");
                return result;
            }

            //check if class exists
            var @class = await _classRepo.GetAll().Where(x => x.Id == model.ClassId).FirstOrDefaultAsync();
            if (@class == null)
            {
                result.AddError("class does not exists");
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
            _unitOfWork.BeginTransaction();

            var oldClass = stud.ClassId;

            stud.ClassId = model.ClassId;


            stud.User.FirstName = model.FirstName;
            stud.User.LastName = model.LastName;
               stud.User.Email = model.ContactEmail.Trim();
            stud.User.NormalizedEmail = model.ContactEmail.Trim().ToUpper();
            stud.User.UserName = stud.RegNumber;
            stud.User.NormalizedUserName = stud.RegNumber.ToUpper();
            stud.User.PhoneNumber = model.ContactPhone;
            stud.User.UserType = UserType.Student;

            //TODO: add more props
            var medicalHistory = new MedicalDetail
            {
                Allergies = model.Allergies,
                BloodGroup = model.BloodGroup,
                ConfidentialNotes = model.ConfidentialNotes,
                Disability = model.Disability,
                Genotype = model.Genotype
            };
            var immunizations = new List<ImmunizationHistory>();

            foreach (var im in model.ImmunizationVms)
            {
                immunizations.Add(new ImmunizationHistory
                {
                    Age = im.Age,
                    DateImmunized = im.DateImmunized,
                    Vaccine = im.Vaccine
                });
            }

            medicalHistory.ImmunizationHistories = immunizations;


            stud. Address = model.ContactAddress;
            stud. AdmissionDate = model.AdmissionDate;
            stud.ClassId = model.ClassId;
            stud.Country = model.ContactCountry;
            stud.DateOfBirth = model.DateOfBirth;
            stud.EntryType = model.EntryType;
            stud.Level = model.Level;
            stud.LocalGovernment = model.LocalGovt;
            stud.MedicalDetail = medicalHistory;
            stud.MothersMaidenName = model.MothersMaidenName;
            stud.Nationality = model.Nationality;
            stud.ParentId = model.ParentId;
            stud.TransportRoute = model.TransportRoute;
            stud.Religion = model.Religion;
            stud.Sex = model.Sex;
            stud.State = model.ContactState;
            stud.StateOfOrigin = model.StateOfOrigin;
            stud.StudentType = model.StudentType;
            stud.Town = model.ContactTown;
            stud.IsActive = model.IsActive;

            if (model.Files != null)
            {
                stud.FileUploads = files;
            }

            await _studentRepo.UpdateAsync(stud);
            await _unitOfWork.SaveChangesAsync();

          

            _unitOfWork.Commit();

            //add / update classId to claims
            var user = await _userManager.FindByIdAsync(stud.UserId.ToString());
            var claims = await _userManager.GetClaimsAsync(user);

            if (claims.Any(m => m.Type == ClaimsKey.StudentClassId))
            {
                await _userManager.ReplaceClaimAsync(
                    user,
                    new System.Security.Claims.Claim(ClaimsKey.StudentClassId, claims.Single(m => m.Type == ClaimsKey.StudentClassId).Value),
                    new System.Security.Claims.Claim(ClaimsKey.StudentClassId, model.ClassId.ToString())
                    );
            }
            else
            {
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.StudentClassId, model.ClassId.ToString()));
            }

            ////PublishMessage
            await _publishService.PublishMessage(Topics.Student, BusMessageTypes.STUDENT, new StudentSharedModel
            {
                IsActive = true,
                ClassId = stud.ClassId,
                TenantId = stud.TenantId,
                UserId = stud.UserId,
                FirstName = stud.User.FirstName,
                LastName = stud.User.LastName,
                Email = stud.User.Email,
                Phone = stud.User.PhoneNumber,
                RegNumber= stud.RegNumber,
                ParentName = $"{parent.User.FirstName} {parent.User.LastName}",
                ParentEmail = parent.User.Email,
                ParentId = parent.Id,
                Sex = stud.Sex,
                DoB = stud.DateOfBirth,
            });

            result.Data = stud;
            return result;
        }
    }
}