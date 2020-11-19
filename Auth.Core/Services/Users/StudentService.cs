using Auth.Core.Models;
using Auth.Core.Models.Medical;
using Auth.Core.Models.Users;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.Student;
using IPagedList;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Entities;
using Shared.Enums;
using Shared.FileStorage;
using Shared.Pagination;
using Shared.PubSub;
using Shared.Utils;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public StudentService(
            IRepository<Student, long> studentRepo,
            IRepository<Parent, long> parentRepo,
            IRepository<SchoolClass, long> classRepo,
            IDocumentService documentService,
            IUnitOfWork unitOfWork,
            IPublishService publishService,
            UserManager<User> userManager)
        {
            _studentRepo = studentRepo;
            _classRepo = classRepo;
            _parentRepo = parentRepo;
            _unitOfWork = unitOfWork;
            _documentService = documentService;
            _publishService = publishService;
            _userManager = userManager;
        }

        public async Task<ResultModel<StudentVM>> AddStudentToSchool(CreateStudentVM model)
        {
            var result = new ResultModel<StudentVM>();

            _unitOfWork.BeginTransaction();

            //check if parent exists
            var parent = await _parentRepo.GetAll().Where(x => x.Id == model.ParentId).FirstOrDefaultAsync();

            if (parent == null)
            {
                result.AddError("No parent exists");
                return result;
            }

            //check if class exists
            var @class = await _classRepo.GetAll().Where(x => x.Id == model.ClassId).FirstOrDefaultAsync();
            if (@class == null)
            {
                result.AddError("class exists");
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
                Email = model.ContactEmail,
                UserName = model.ContactEmail,
                PhoneNumber = model.ContactPhone,
                UserType = UserType.Student,
            };

            var userResult = await _userManager.CreateAsync(user, model.ContactPhone);

            if (!userResult.Succeeded)
            {
                result.AddError(string.Join(';', userResult.Errors.Select(x => x.Description)));
                return result;
            }
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
                Town = model.ContactTown
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
                          .Include(x => x.User);

            var pagedData = await query.ToPagedListAsync(model.PageIndex, model.PageSize);

            result.Data = new PaginatedModel<StudentVM>(pagedData.Select(x => (StudentVM)x), model.PageIndex, model.PageSize, pagedData.TotalItemCount);

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