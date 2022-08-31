using Auth.Core.Context;
using Auth.Core.Interfaces.Setup;
using Auth.Core.Models;
using Auth.Core.Models.Alumni;
using Auth.Core.Models.Medical;
using Auth.Core.Models.Users;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.Student;
using ClosedXML.Excel;
using ExcelManager;
using IPagedList;
using Microsoft.AspNetCore.Http;
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
using System.IO;
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
        private readonly IRepository<Alumni, long> _alumniRepo;
        private readonly AppDbContext _context;
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
            IAuthUserManagement authUserManagement,
            IRepository<Alumni, long> alumniRepo, AppDbContext context
            )
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
            _alumniRepo = alumniRepo;
            _context=context;
        }

        private async Task<Student> SaveStudentWithSystemRegNumber(Student student, string schoolSeperator, string schoolPrefix)
        {
            //var lastRegNumber = _studentRepo.GetAll().OrderBy(m => m.Id).Select(m => m.RegNumber).LastOrDefaultAsync().Result;
            var klastRegNumber = _context.Users.Where(x => x.UserType == UserType.Student && x.UserName.Contains(schoolPrefix)).OrderBy(m => m.Id).Select(m => m.UserName).LastOrDefaultAsync().Result;
            var lastNumber = 0;
            var lastRegNumber = klastRegNumber.ToString();
            var seperator = schoolSeperator;
            if (!string.IsNullOrWhiteSpace(lastRegNumber))
            {
                lastNumber = int.Parse(lastRegNumber.Split(seperator).Last());
            }
            var nextNumber = lastNumber;
            var saved = false;
            var firstTime = true;

            while (!saved)
            {
                try
                {
                    nextNumber++;

                    if (firstTime && !string.IsNullOrWhiteSpace(student.RegNumber))
                    {
                        firstTime = false;
                        _studentRepo.Insert(student);
                        _unitOfWork.SaveChanges();
                    }
                    else
                    {
                        student.RegNumber = $"{schoolPrefix}{seperator}STT{seperator}{DateTime.Now.Year}{seperator}{nextNumber:00000}";

                        firstTime = false;
                        _studentRepo.Insert(student);
                        _unitOfWork.SaveChanges();
                    }


                    saved = true;
                }
                // 2627 is unique constraint (includes primary key), 2601 is unique index
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException && (sqlException.Number == 2627 || sqlException.Number == 2601))
                {
                    saved = false;
                }
            }

            return student;
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
            var parent = await _parentRepo.GetAll().Include(m => m.User)
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

            //var existingUser = await _userManager.FindByEmailAsync(user.Email);
            IdentityResult userResult;

            userResult = await _userManager.CreateAsync(user, model.ContactPhone);


            if (!userResult.Succeeded)
            {
                result.AddError(string.Join(';', userResult.Errors.Select(x => x.Description)));
                return result;
            }

            //Add TenantId to UserClaims
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.TenantId, _httpUserService.GetCurrentUser().TenantId?.ToString()));
            //add stafftype to claims
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.UserType, UserType.Student.GetDescription()));

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

            var medicalHistory = new MedicalDetail
            {
                Allergies = model.Allergies,
                BloodGroup = model.BloodGroup,
                ConfidentialNotes = model.ConfidentialNotes,
                Disability = model.Disability,
                Genotype = model.Genotype,
                ImmunizationHistories = immunizations
            };
            // medicalHistory.ImmunizationHistories = immunizations;

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

            stud = await SaveStudentWithSystemRegNumber(stud, schoolProperty.Data.Seperator, schoolProperty.Data.Prefix);

            //change user's username to reg number
            user.UserName = stud.RegNumber;
            user.NormalizedUserName = stud.RegNumber.ToUpper();
            await _userManager.UpdateAsync(user);

            //add classId to claims
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.StudentClassId, stud.ClassId.ToString()));

            _unitOfWork.Commit();

            var school = await _schoolRepo.GetAll().Where(m => m.Id == stud.TenantId).Include(x => x.SchoolContactDetails).FirstOrDefaultAsync();
            var contactDetails = school.SchoolContactDetails.Where(m => m.SchoolId == schoolProperty.Data.TenantId).FirstOrDefault();
            //broadcast login detail to email
            var emailResult = await _authUserManagement.SendRegistrationEmail(user, school.DomainName, school.Name, contactDetails.Email, school.Address, contactDetails.PhoneNumber, contactDetails.EmailPassword);

            if (emailResult.HasError)
            {
                return new ResultModel<StudentVM>(emailResult.ErrorMessages);
            }
            //PublishMessage
            await _publishService.PublishMessage(Topics.Student, BusMessageTypes.STUDENT, new List<StudentSharedModel>{ new StudentSharedModel
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
                StudentStatusInSchool = stud.StudentStatusInSchool,
            } });

            result.Data = new StudentVM
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = stud.DateOfBirth,
                Id = stud.Id,
                UserId = stud.UserId,
                StudentNumber = user.UserName
            };
            return result;
        }

        public async Task<ResultModel<bool>> DeleteStudent(long userId,DeactivateStudent model)
        {
            var result = new ResultModel<bool> { Data = false };

            //check if the student exists
            var std = await _studentRepo.GetAll().Include(x => x.Parent).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == userId && x.IsDeleted == false);

            if (std == null)
            {
                result.AddError("Student not found");
                return result;
            }

            //delete auth user
            std.IsDeleted = true;
            std.DeletionTime = DateTime.Now;
            await _studentRepo.UpdateAsync(std);


            //update the isdeleted field in user table as well
            var id = std.UserId.ToString();
            var findUser = await _userManager.FindByIdAsync(id);
            findUser.UserStatus = UserStatus.Deactivated;


            var updateUser = await _userManager.UpdateAsync(findUser);

            var alumni = new Alumni(std, model.SessionName, model.DeactivationReason);
            alumni = await _alumniRepo.InsertAsync(alumni);

            await _unitOfWork.SaveChangesAsync();
            ////PublishMessage
            await _publishService.PublishMessage(Topics.Student, BusMessageTypes.STUDENT_UPDATE, new List<StudentSharedModel>{
                new StudentSharedModel
                {
                    Id = std.Id,
                    RegNumber = std.RegNumber,
                    IsActive = true,
                    ClassId = std.ClassId,
                    TenantId = std.TenantId,
                    UserId = std.UserId,
                    FirstName = std.User.FirstName,
                    LastName = std.User.LastName,
                    Email = std.User.Email,
                    Phone = std.User.PhoneNumber,
                    //ParentName = $"{std.Parent.User.FirstName} {std.Parent.User.LastName}",
                    //ParentEmail = std.Parent.User.Email,
                    ParentId = std.Parent.Id,
                    Sex = std.Sex,
                    DoB = std.DateOfBirth,
                    StudentStatusInSchool = std.StudentStatusInSchool,
                    IsDeleted = std.IsDeleted
                }
            });

            result.Data = true;
            return result;
        }

        public async Task<ResultModel<PaginatedModel<StudentVMs>>> GetAllStudentsInSchool(QueryModel model)
        {

            var resultModel = new ResultModel<PaginatedModel<StudentVMs>>();

            var query = await _studentRepo.GetAll().Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationTime)
                .Select(x => new
                {
                    x.Id,
                    x.User.FirstName,
                    x.User.LastName,
                    x.Sex,
                    x.DateOfBirth,
                    section = x.Class.SchoolSection.Name,
                    x.IsActive,
                    x.RegNumber,
                    x.Class,
                    image = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName()).Path
                }).ToListAsync();

            if (query != null)
            {
                var student = query.Select(x => new StudentVMs
                {

                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Sex = x.Sex,
                    DateOfBirth = x.DateOfBirth,
                    Section = x.section,
                    StudentNumber = x.RegNumber,
                    SchoolClass = x.Class,
                    Image = x.image == null ? null : _documentService.TryGetUploadedFile(x.image)
                }).ToList();


                var data = student.ToPagedList(model.PageIndex, model.PageSize);

                resultModel.Data = new PaginatedModel<StudentVMs>(data, model.PageIndex, model.PageSize, query.Count);


                return resultModel;
            }

            return resultModel;
        }

        public async Task<ResultModel<PaginatedModel<StudentVM>>> GetAllStudentsInClass(QueryModel model, long classId)
        {
            var query = _studentRepo.GetAll()
                .Where(x => x.ClassId == classId && x.IsDeleted == false)
                .Select(x => new StudentVM
                {
                    Id = x.Id,
                    SchoolClass = x.Class,
                    DateOfBirth = x.DateOfBirth,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    StudentNumber = x.RegNumber,
                    Sex = x.Sex,
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber,
                    Section = x.Class.SchoolSection.Name,
                    IsActive = x.IsActive,
                    ImagePath = x.FileUploads.Where(fileUpload => fileUpload.Name == DocumentType.ProfilePhoto.GetDisplayName()).Select(x => x.Path).FirstOrDefault()
                }); ;

            var pagedData = await query.ToPagedListAsync(model.PageIndex, model.PageSize);


            return new ResultModel<PaginatedModel<StudentVM>>(data: new PaginatedModel<StudentVM>(pagedData, model.PageIndex, model.PageSize, pagedData.TotalItemCount));
        }

        public async Task<ResultModel<StudentDetailVM>> GetStudentById(long Id)
        {
            var result = new ResultModel<StudentDetailVM>();
            var std = await _studentRepo.GetAll().Where(x => x.Id == Id && x.IsDeleted == false)
                        .Select(x => new
                        {
                            x.TenantId,
                            x.Id,
                            x.User.FirstName,
                            x.User.LastName,
                            x.MothersMaidenName,
                            x.Sex,
                            x.RegNumber,
                            x.DateOfBirth,
                            ParentFirstName = x.Parent.User.FirstName,
                            ParentLastName = x.Parent.User.LastName,
                            x.ParentId,
                            x.Nationality,
                            x.Religion,
                            SchoolSectionid = x.Class.SchoolSectionId,
                            x.LocalGovernment,
                            x.StateOfOrigin,
                            x.EntryType,
                            x.AdmissionDate,
                            x.Level,
                            SchoolClass = x.Class,
                            Section = x.Class.SchoolSection.Name,
                            x.StudentType,
                            x.MedicalDetail.BloodGroup,
                            x.MedicalDetail.Genotype,
                            x.MedicalDetail.Allergies,
                            x.MedicalDetail.ConfidentialNotes,
                            x.MedicalDetail.Disability,
                            Immunization = x.MedicalDetail.ImmunizationHistories,
                            x.User.PhoneNumber,
                            x.User.Email,
                            x.Country,
                            x.Address,
                            x.Town,
                            x.State,
                            x.IsActive,
                            image = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName()),
                            x.UserId
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

            var data = new StudentDetailVM()
            {
                StudentType = std.StudentType.GetDisplayName(),
                StateOfOrigin = std.StateOfOrigin,
                State = std.State,
                Sex = std.Sex,
                RegNumber = std.RegNumber,
                Section = std.Section,
                AdmissionDate = std.AdmissionDate,
                Allergies = std.Allergies,
                Disability = std.Disability,
                BloodGroup = std.BloodGroup,
                SchoolClass = std.SchoolClass,
                City = std.Town,
                ConfidentialNote = std.ConfidentialNotes,
                DateOfBirth = std.DateOfBirth,
                Country = std.Country,
                EmailAddress = std.Email,
                FirstName = std.FirstName,
                Genotype = std.Genotype,
                HomeAddress = std.Address,
                Id = std.Id,
                Level = std.Level,
                EntryType = std.EntryType,
                SectionId = std.SchoolSectionid,
                ParentId = std.ParentId,
                Image = _documentService.TryGetUploadedFile(std.image?.Path),
                ImmunizationHistoryVMs = std.Immunization.Select(x => new ImmunizationHistoryVM { Age = x.Age, DateImmunized = x.DateImmunized, Vaccine = x.Vaccine }).ToList(),
                LastName = std.LastName,
                LocalGovernment = std.LocalGovernment,
                MothersMaidenName = std.MothersMaidenName,
                Nationality = std.Nationality,
                ParentFirstName = std.ParentFirstName,
                ParentLastName = std.ParentLastName,
                PhoneNumber = std.PhoneNumber,
                Religion = std.Religion,
                IsActive = std.IsActive,
                UserId = std.UserId
            };

            result.Data = data;
            return result;
        }

        public async Task<ResultModel<StudentDetailVM>> GetStudentProfileByUserId(long Id)
        {
            var result = new ResultModel<StudentDetailVM>();
            var std = await _studentRepo.GetAll().Where(x => x.UserId == Id && x.IsDeleted == false)
                        .Select(x => new
                        {
                            x.Id,
                            x.RegNumber,
                            x.User.FirstName,
                            x.User.LastName,
                            x.MothersMaidenName,
                            x.Sex,
                            x.DateOfBirth,
                            ParentFirstName = x.Parent.User.FirstName,
                            ParentLastName = x.Parent.User.LastName,
                            x.Nationality,
                            x.Religion,
                            x.LocalGovernment,
                            x.StateOfOrigin,
                            x.EntryType,
                            x.AdmissionDate,
                            x.Level,
                            ClassName = x.Class,
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
                SchoolClass = std.ClassName,
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
                ParentFirstName = std.ParentFirstName,
                ParentLastName = std.ParentLastName,
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
                .Where(x => x.Id == Id && x.IsDeleted == false)
                .Include(x => x.User)
                .Include(c => c.Class)
                .FirstOrDefaultAsync();

            if (stud == null)
            {
                result.AddError("Student does not exist");
                return result;
            }

            //check if parent exists
            var parent = await _parentRepo.GetAll().Include(m => m.User)
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


            stud.Address = model.ContactAddress;
            stud.AdmissionDate = model.AdmissionDate;
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

            var classClaims = claims.Where(m => m.Type == ClaimsKey.StudentClassId);

            if (classClaims.Any())
            {
                await _userManager.RemoveClaimsAsync(user, classClaims);
            }

            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.StudentClassId, model.ClassId.ToString()));


            ////PublishMessage
            await _publishService.PublishMessage(Topics.Student, BusMessageTypes.STUDENT, new List<StudentSharedModel>{
                new StudentSharedModel
                {
                    Id = stud.Id,
                    RegNumber = stud.RegNumber,
                    IsActive = true,
                    ClassId = stud.ClassId,
                    TenantId = stud.TenantId,
                    UserId = stud.UserId,
                    FirstName = stud.User.FirstName,
                    LastName = stud.User.LastName,
                    Email = stud.User.Email,
                    Phone = stud.User.PhoneNumber,
                    ParentName = $"{parent.User.FirstName} {parent.User.LastName}",
                    ParentEmail = parent.User.Email,
                    ParentId = parent.Id,
                    Sex = model.Sex,
                    DoB = model.DateOfBirth,
                    StudentStatusInSchool = stud.StudentStatusInSchool,
                }
            });

            result.Data = stud;
            return result;
        }

        public async Task<ResultModel<byte[]>> GetStudentsExcelSheet()
        {

            var data = new StudentBulkUploadExcel().ToExcel("Student");

            if (data == null)
            {
                return new ResultModel<byte[]>("An error occurred while generating excel");
            }
            else
            {
                return new ResultModel<byte[]>(data);
            }
        }

        public async Task<ResultModel<bool>> AddBulkStudent(IFormFile excelfile)
        {
            var result = new ResultModel<bool>();

            var schoolProperty = await _schoolPropertyService.GetSchoolProperty();
            if (schoolProperty.HasError)
            {
                result.AddError(schoolProperty.ValidationErrors);
                return result;
            }


            var importedData = ExcelReader.FromExcel<StudentBulkUploadExcel>(excelfile);

            //check if imported data contains any data
            if (importedData.Count < 1)
            {

                result.AddError("No data was imported");

                return result;
            }

            var students = new List<Student>();

            _unitOfWork.BeginTransaction();

            foreach (var model in importedData)
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.ContactEmail.Trim(),
                    UserName = model.ContactEmail.Trim(),
                    PhoneNumber = model.ContactPhone,
                    UserType = UserType.Student
                };


                IdentityResult userResult;
                userResult = await _userManager.CreateAsync(user, model.ContactPhone);


                if (!userResult.Succeeded)
                {
                    result.AddError(string.Join(';', userResult.Errors.Select(x => x.Description)));
                    return result;
                }

                //Add TenantId to UserClaims
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.TenantId, _httpUserService.GetCurrentUser().TenantId?.ToString()));
                //add stafftype to claims
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.UserType, UserType.Student.GetDescription()));

                var medicalHistory = new MedicalDetail
                {
                    Allergies = model.Allergies,
                    BloodGroup = model.BloodGroup,
                    ConfidentialNotes = model.ConfidentialNotes,
                    Disability = model.Disability,
                    Genotype = model.Genotype
                };
                //todo : add more props

                var student = new Student
                {
                    UserId = user.Id,
                    State = model.ContactState,
                    Address = model.ContactAddress,
                    TransportRoute = model.TransportRoute,
                    Town = model.ContactTown,
                    Country = model.ContactCountry,
                    Nationality = model.Nationality,
                    Religion = model.Religion,
                    EntryType = model.EntryType,
                    StudentType = model.StudentType,
                    AdmissionDate = model.AdmissionDate,
                    IsActive = true,
                    StateOfOrigin = model.StateOfOrigin,
                    LocalGovernment = model.LocalGovt,
                    Sex = model.Sex,
                    DateOfBirth = model.DateOfBirth,
                    MothersMaidenName = model.MothersMaidenName,
                    StudentStatusInSchool = StudentStatusInSchool.IsStudent,
                    MedicalDetail = medicalHistory
                };

                student = await SaveStudentWithSystemRegNumber(student, schoolProperty.Data.Seperator, schoolProperty.Data.Prefix);

                //change user's username to reg number
                user.UserName = student.RegNumber;
                user.NormalizedUserName = student.RegNumber.ToUpper();
                await _userManager.UpdateAsync(user);

                //add classId to claims
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.StudentClassId, student.ClassId.ToString()));


                var school = await _schoolRepo.GetAll().Where(m => m.Id == schoolProperty.Data.TenantId).Include(x => x.SchoolContactDetails).FirstOrDefaultAsync();
                var contactdetails = school.SchoolContactDetails.Where(m => m.SchoolId == schoolProperty.Data.TenantId).FirstOrDefault();

                //broadcast login detail to email
                var emailResult = await _authUserManagement.SendRegistrationEmail(user, school.DomainName, school.Name, contactdetails.Email, school.Address, contactdetails.PhoneNumber, contactdetails.EmailPassword);

                if (emailResult.HasError)
                {
                    return new ResultModel<bool>(emailResult.ErrorMessages);
                }
                students.Add(student);
            }

            _unitOfWork.Commit();

            foreach (var student in students)
            {
                //publish to services
                await _publishService.PublishMessage(Topics.Student, BusMessageTypes.STUDENT, new List<StudentSharedModel>
                { new StudentSharedModel
                    {
                        Id = student.Id,
                        IsActive = true,
                        ClassId = student.ClassId,
                        TenantId = student.TenantId,
                        UserId = student.UserId,
                        DoB = student.DateOfBirth,
                        Sex = student.Sex,
                        RegNumber = student.RegNumber,
                        StudentStatusInSchool = student.StudentStatusInSchool,
                    }
                });
            }
            result.Data = true;

            return result;
        }

        public async Task<ResultModel<PaginatedModel<StudentVMs>>> SearchForStudentByName(QueryModel model,string name)
        {
            var resultModel = new ResultModel<PaginatedModel<StudentVMs>>();

            var query = await _studentRepo.GetAll().Where(x => x.User.FirstName.Contains(name) || x.User.LastName.Contains(name) && x.IsDeleted == false)
                .Select(x => new {
                    x.Id,
                    x.User.FirstName,
                    x.User.LastName,
                    x.Sex,
                    x.DateOfBirth,
                    section = x.Class.SchoolSection.Name,
                    x.IsActive,
                    x.RegNumber,
                    x.Class,
                    image = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName()).Path
                }).ToListAsync();

            if (query != null)
            {
                var student = query.Select(x => new StudentVMs
                {

                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Sex = x.Sex,
                    DateOfBirth = x.DateOfBirth,
                    Section = x.section,
                    StudentNumber = x.RegNumber,
                    SchoolClass = x.Class,
                    Image = x.image == null ? null : _documentService.TryGetUploadedFile(x.image)
                }).ToList();

                var data = student.ToPagedList(model.PageIndex, model.PageSize);

                resultModel.Data = new PaginatedModel<StudentVMs>(data, model.PageIndex, model.PageSize, query.Count);

            }

            return resultModel;

        }

        public async Task<ResultModel<ExportPayloadVM>> ExportStudentData(StudentExportVM model)
        {
            var resultModel = new ResultModel<ExportPayloadVM>();

            var query = await _studentRepo.GetAllIncluding(x => x.Class)
                .Include(x => x.User)
                .Include(x => x.Parent)
                .Include(x => x.MedicalDetail)
                .ToListAsync();


            if (model.classId != null)
            {
                query = query.Where(x => x.ClassId == model.classId).ToList();
            }

            if (query == null)
            {
                resultModel.Data = null;
                resultModel.Message = "No Student Found";
                return resultModel;
            }

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var workSheet = workbook.Worksheets.Add("AttendanceSheet");

                    for (int i = 1; i <= 11; i++)
                    {
                        var headFormat = workSheet.Cell(1, i);
                        headFormat.Style.Font.SetBold();
                        headFormat.WorksheetRow().Height = 11;
                    }

                    var currentRow = 1;

                    workSheet.Cell(1, 1).Value = "FirstName";
                    workSheet.Cell(1, 2).Value = "LastName";
                    workSheet.Cell(1, 3).Value = "ClassName";
                    workSheet.Cell(1, 4).Value = "StudentId";
                    workSheet.Cell(1, 5).Value = "Address";
                    workSheet.Cell(1, 6).Value = "State";
                    workSheet.Cell(1, 7).Value = "Country";
                    workSheet.Cell(1, 8).Value = "ParentFullName";
                    workSheet.Cell(1, 9).Value = "MedicalBloodGroup";
                    workSheet.Cell(1, 10).Value = "MedicalGenotype";
                    workSheet.Cell(1, 11).Value = "Religion";


                    foreach (var data in query)
                    {
                        var parent = await _parentRepo.GetAllIncluding(x => x.User).Where(x => x.Id == data.Parent.Id).FirstOrDefaultAsync();

                        currentRow += 1;
                        workSheet.Cell(currentRow, 1).Value = $"{data.User.FirstName}";
                        workSheet.Cell(currentRow, 2).Value = $"{data.User.LastName}";
                        workSheet.Cell(currentRow, 3).Value = $"{data.Class.Name} {data.Class.ClassArm}";
                        workSheet.Cell(currentRow, 4).Value = $"{data.Id}";
                        workSheet.Cell(currentRow, 5).Value = $"{data.Address}";
                        workSheet.Cell(currentRow, 6).Value = $"{data.State}";
                        workSheet.Cell(currentRow, 7).Value = $"{data.Country}";
                        workSheet.Cell(currentRow, 8).Value = $"{parent.User.FirstName} {parent.User.LastName}";
                        workSheet.Cell(currentRow, 9).Value = $"{data.MedicalDetail.BloodGroup}";
                        workSheet.Cell(currentRow, 10).Value = $"{data.MedicalDetail.Genotype}";
                        workSheet.Cell(currentRow, 11).Value = $"{data.Religion}";
                    }
                    var byteData = new byte[0];
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        byteData = content;
                    }

                    var payload = new ExportPayloadVM
                    {
                        FileName = "StudentData",
                        Base64String = Convert.ToBase64String(byteData)
                    };

                    resultModel.Data = payload;
                }
            }
            catch (Exception ex)
            {
                resultModel.AddError($"Exception Occured : {ex.Message}");
                return resultModel;
            }

            return resultModel;
        }


        public async Task<ResultModel<PaginatedModel<StudentVMs>>> GetStudentByClass(StudentExportVM classVM, QueryModel model)
        {
            var resultModel = new ResultModel<PaginatedModel<StudentVMs>>();

            var query = await _studentRepo.GetAll().Where(x => x.IsDeleted == false).Include(x => x.Class).OrderByDescending(x => x.CreationTime)
                .Select(x => new
                {
                    x.Id,
                    x.User.FirstName,
                    x.User.LastName,
                    x.Sex,
                    x.DateOfBirth,
                    section = x.Class.SchoolSection.Name,
                    x.IsActive,
                    x.RegNumber,
                    x.Class,
                    image = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName()).Path
                }).ToListAsync();

            if (query != null)
            {
                if (model != null)
                {
                    query = query.Where(x => x.Class.Id == classVM.classId).ToList();
                }
                
                var student = query.Select(x => new StudentVMs
                {

                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Sex = x.Sex,
                    DateOfBirth = x.DateOfBirth,
                    Section = x.section,
                    StudentNumber = x.RegNumber,
                    SchoolClass = x.Class,
                    Image = x.image == null ? null : _documentService.TryGetUploadedFile(x.image)
                }).ToList();


                var data = student.ToPagedList(model.PageIndex, model.PageSize);

                resultModel.Data = new PaginatedModel<StudentVMs>(data, model.PageIndex, model.PageSize, query.Count);


                return resultModel;
            }

            return resultModel;
        }
    }
}