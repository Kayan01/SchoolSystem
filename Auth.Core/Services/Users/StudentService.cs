﻿using Auth.Core.Models;
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
using Shared.FileStorage;
using Shared.Pagination;
using Shared.PubSub;
using Shared.Tenancy;
using Shared.Utils;
using Shared.ViewModels;
using System.Collections.Generic;
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
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IHttpUserService _httpUserService;

        public StudentService(
            IRepository<Student, long> studentRepo,
            IRepository<Parent, long> parentRepo,
            IRepository<SchoolClass, long> classRepo,
            IDocumentService documentService,
            IUnitOfWork unitOfWork,
            IPublishService publishService,
            IHttpUserService httpUserService,
            UserManager<User> userManager)
        {
            _studentRepo = studentRepo;
            _classRepo = classRepo;
            _parentRepo = parentRepo;
            _unitOfWork = unitOfWork;
            _documentService = documentService;
            _publishService = publishService;
            _userManager = userManager;
            _httpUserService = httpUserService;
        }

        public async Task<ResultModel<StudentVM>> AddStudentToSchool(CreateStudentVM model)
        {
            var result = new ResultModel<StudentVM>();

            _unitOfWork.BeginTransaction();

            //check if parent exists
            var parent = await _parentRepo.GetAll()
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

            var stud = _studentRepo.Insert(new Student
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
            });

            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.Commit();

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
                Email = model.ContactEmail,
                Phone = model.ContactPhone
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
                .Select(x => new StudentVM
                {
                     Id = x.Id,
                    Class = x.Class.FullName,
                    DateOfBirth = x.DateOfBirth,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    StudentNumber = $"AMI/ST/2020/{x.Id}",
                    Sex = x.Sex,
                    Section = x.Class.SchoolSection.Name,
                    IsActive = x.IsActive,
                    ImagePath = x.FileUploads.Where(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName()).Select(x => x.Path).FirstOrDefault()
                });

            var pagedData = await query.ToPagedListAsync(model.PageIndex, model.PageSize);
            

            result.Data = new PaginatedModel<StudentVM>(pagedData, model.PageIndex, model.PageSize, pagedData.TotalItemCount);

            return result;
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
                            x.DateOfBirth,
                            ParentName =  x.Parent.User.FullName,
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
                            Immunization = x.MedicalDetail.ImmunizationHistories.Select(x => new { x.DateImmunized, x.Vaccine }),
                            x.User.PhoneNumber,
                            x.User.Email,
                            x.Country,
                            x.Address,
                            x.Town,
                            x.State,
                            files = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName())
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
                ImagePath = std.files?.Path,
                Immunization = sb.ToString(),
                LastName = std.LastName,
                LocalGovernment = std.LocalGovernment,
                MothersMaidenName = std.MothersMaidenName,
                Nationality = std.Nationality,
                ParentName = std.ParentName,
                PhoneNumber = std.PhoneNumber,
                Religion = std.Religion,

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
                            Immunization = x.MedicalDetail.ImmunizationHistories.Select(x => new { x.DateImmunized, x.Vaccine }),
                            x.User.PhoneNumber,
                            x.User.Email,
                            x.Country,
                            x.Address,
                            x.Town,
                            x.State,
                            files = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName())
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
                ImagePath = std.files?.Path,
                Immunization = sb.ToString(),
                LastName = std.LastName,
                LocalGovernment = std.LocalGovernment,
                MothersMaidenName = std.MothersMaidenName,
                Nationality = std.Nationality,
                ParentName = std.ParentName,
                PhoneNumber = std.PhoneNumber,
                Religion = std.Religion,

            };
            return result;
        }

        public async Task<ResultModel<StudentVM>> UpdateStudent(StudentUpdateVM model)
        {
            var result = new ResultModel<StudentVM>();

            var stud = await _studentRepo.GetAll()
                .Include(x => x.User)
                .Include(c => c.Class)
                .FirstOrDefaultAsync(x => x.Id == model.UserId);

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