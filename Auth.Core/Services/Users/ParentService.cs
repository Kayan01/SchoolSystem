using Auth.Core.Interfaces.Setup;
using Auth.Core.Interfaces.Users;
using Auth.Core.Models;
using Auth.Core.Models.Users;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.Parent;
using Auth.Core.ViewModels.School;
using Auth.Core.ViewModels.Student;
using IPagedList;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Entities;
using Shared.Enums;
using Shared.Extensions;
using Shared.FileStorage;
using Shared.Pagination;
using Shared.PubSub;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Shared.Utils.CoreConstants;

namespace Auth.Core.Services.Users
{
    public class ParentService : IParentService
    {
        private readonly IPublishService _publishService;
        private readonly IRepository<Parent, long> _parentRepo;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IRepository<School, long> _schoolRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IDocumentService _documentService;
        private readonly ISchoolPropertyService _schoolPropertyService;

        private readonly IAuthUserManagement _authUserManagementService;
        public ParentService(
            IPublishService publishService,
            IRepository<Parent, long> parentRepo,
            IUnitOfWork unitOfWork,
            IRepository<Student, long> studentRepo,
            IRepository<School, long> schoolRepo,
            UserManager<User> userManager,
            IDocumentService documentService,
            ISchoolPropertyService schoolPropertyService,
            IAuthUserManagement authUserManagementService)
        {
            _publishService = publishService;
            _parentRepo = parentRepo;
            _unitOfWork = unitOfWork;
            _studentRepo = studentRepo;
            _userManager = userManager;
            _documentService = documentService;
            _schoolRepo = schoolRepo;
            _authUserManagementService = authUserManagementService;
            _schoolPropertyService = schoolPropertyService;
        }
        public async Task<ResultModel<string>> DeleteParent(long Id)
        {
            var resultModel = new ResultModel<string>();

            var parent = await _parentRepo.FirstOrDefaultAsync(Id);

            if (parent == null)
            {
                resultModel.AddError($"No parent for id : {Id}");
                return resultModel;
            }

           await _parentRepo.DeleteAsync(parent);

           await _unitOfWork.SaveChangesAsync();

            resultModel.Data = "Deleted";
            return resultModel;
        }

        public async Task<ResultModel<PaginatedModel<ParentListVM>>> GetAllParents(QueryModel vm)
        {

            var resultModel = new ResultModel<PaginatedModel<ParentListVM>>();

            var query =  _parentRepo.GetAll()
                .Include(x => x.User)
                .Include(x=> x.Students)
                .Include(x => x.FileUploads);

            var parents = await query.ToPagedListAsync(vm.PageIndex, vm.PageSize);

            var data = new PaginatedModel<ParentListVM>(parents.Select(x=> (ParentListVM)x), vm.PageIndex, vm.PageSize, parents.TotalItemCount);

            resultModel.Data = data;

            return resultModel;
        }

        public async Task<ResultModel<PaginatedModel<ParentListVM>>> GetAllParentsInSchool(QueryModel vm)
        {

            var resultModel = new ResultModel<PaginatedModel<ParentListVM>>();

            var query =  await _studentRepo.GetAll()
                .Include(x => x.Parent)
                .Select(x => new
                {
                    Email = x.Parent.User.Email,
                    FullName = x.Parent.User.FullName,
                    Id = x.ParentId,
                    ParentCode = $"PRT/{x.Parent.CreationTime.Year}/{x.Parent.Id}",
                    PhoneNumber = x.Parent.User.PhoneNumber,
                    Status = x.Parent.Status,
                    Image = x.Parent.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName()).Path
                }).ToListAsync();

            var parents = query.GroupBy(x => x.Id).Select(x => new ParentListVM
            {
                Email = x.FirstOrDefault()?.Email,
                FullName = x.FirstOrDefault()?.FullName,
                ParentCode = x.FirstOrDefault()?.ParentCode,
                Id = x.FirstOrDefault().Id.Value,
                PhoneNumber = x.FirstOrDefault()?.PhoneNumber,
                Status = x.FirstOrDefault().Status,
                Image = x.FirstOrDefault().Image == null ? null : _documentService.TryGetUploadedFile(x.FirstOrDefault().Image),
            }).ToPagedList(vm.PageIndex, vm.PageSize);

            var data = new PaginatedModel<ParentListVM>(parents, vm.PageIndex, vm.PageSize, parents.Count);

            resultModel.Data = data;

            return resultModel;
        }


        public async Task<ResultModel<ParentDetailVM>> GetParentById(long Id)
        {
            var resultModel = new ResultModel<ParentDetailVM>();

            var query = await _parentRepo.GetAll()
                .Where(x=> x.Id == Id)
                .Select(model => new {
                    ContactEmail = model.User.Email,
                    ContactHomeAddress = model.HomeAddress,
                    ContactNumber = model.User.PhoneNumber,
                    model.User.FirstName,
                    model.IdentificationNumber,
                    model.User.LastName,
                    ModeOfIdentification = model.IdentificationType,
                    model.Occupation,
                    OfficeHomeAddress = model.OfficeAddress,
                    model.Sex,
                    model.Title,
                    Children = model.Students.Select(x => new
                    {
                        Id = x.Id,
                        Name = x.User.FullName,
                        logoPath = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName()).Path
                    })
                })
                .FirstOrDefaultAsync();


            if (query == null)
            {
                resultModel.AddError($"No parent for id : {Id}");
                return resultModel;
            }

            var children = query.Children?.Select(x => new ChildView {
                Id = x.Id, 
                Image = _documentService.TryGetUploadedFile(x.logoPath),
                Name = x.Name
            }).ToList();


            var parent = new ParentDetailVM
            {
                Children = children,
                ContactEmail = query.ContactEmail,
                ContactHomeAddress = query.ContactHomeAddress,
                ContactNumber = query.ContactNumber,
                FirstName = query.FirstName,
                IdentificationNumber = query.IdentificationNumber,
                LastName = query.LastName,
                ModeOfIdentification = query.ModeOfIdentification,
                Occupation = query.Occupation,
                OfficeHomeAddress = query.OfficeHomeAddress,
                Sex = query.Sex,
                Title = query.Title,
            };

            resultModel.Data = parent;

            return resultModel;
        }

        public async Task<ResultModel<ParentDetailVM>> GetParentsForStudent(long studId)
        {
            var resultModel = new ResultModel<ParentDetailVM>();

            var student = await _studentRepo.GetAll()
                .Include(x => x.Parent)
                .ThenInclude(x => x.FileUploads)
                .Include(x => x.Parent)
                .ThenInclude(x=> x.User)
                .Where(x => x.Id == studId)
                .FirstOrDefaultAsync();

            if (student == null)
            {
                resultModel.AddError($"No parent for student id : {studId}");
                return resultModel;
            }

            resultModel.Data = student.Parent;

            return resultModel;
        }

        public async Task<ResultModel<List<StudentParentVM>>> GetStudentsInSchool(long parentId)
        {
            var query = await _studentRepo.GetAll()
                .Where(x => x.Parent.UserId == parentId)
                .Select(x => new
                {
                    x.Id,
                    x.User.FullName,
                    x.RegNumber,
                    x.ClassId,
                    ImageId = x.FileUploads.FirstOrDefault(h => h.Name == DocumentType.ProfilePhoto.GetDisplayName()).Path
                })
                .ToListAsync();


            var students = new List<StudentParentVM>();

            foreach (var st in query)
            {
                students.Add(new StudentParentVM
                {
                    FullName = st.FullName,
                    Id = st.Id,
                    ClassID = st.ClassId,
                   // ImageId = st.ImageId,
                    Image = _documentService.TryGetUploadedFile(st.ImageId),
                    RegNo = st.RegNumber
                });
            }

            return new ResultModel<List<StudentParentVM>> { Data = students };


        }

        public async Task<ResultModel<List<SchoolParentViewModel>>> GetStudentsSchools(long currentUserId)
        {
            var query = await _parentRepo.GetAll()
                .Where(x => x.UserId == currentUserId)
                .SelectMany(x =>
                    x.Students.Select(m => new
                    {
                        Id = m.TenantId,
                        ImagePath = m.School.FileUploads.FirstOrDefault(h => h.Name == DocumentType.Logo.GetDisplayName()).Path,
                        Name = m.School.Name
                    }))
                .ToListAsync();

            //removes duplicate  records
            var query2 = query
               .GroupBy(o => o.Id)
               .Select(g => g.First())
               .ToList();

            var schools = new List<SchoolParentViewModel>();
            foreach (var sch in query2)
            {
                schools.Add(new SchoolParentViewModel {
                    Id = sch.Id,
                    Image = _documentService.TryGetUploadedFile(sch.ImagePath),
                    Name = sch.Name
                });
            }

            return new ResultModel<List<SchoolParentViewModel>> { Data = schools };
        }

        public async Task<ResultModel<ParentDetailVM>> AddNewParent(AddParentVM vm)
        {
            var resultModel = new ResultModel<ParentDetailVM>();

            var schoolProperty = await _schoolPropertyService.GetSchoolProperty();
            if (schoolProperty.HasError)
            {
                resultModel.AddError(schoolProperty.ValidationErrors);
                return resultModel;
            }

            _unitOfWork.BeginTransaction();

            var file = new FileUpload();
            //save filles
            if (vm.File != null)
            {

                if (vm.DocumentType != DocumentType.ProfilePhoto)
                {
                    resultModel.AddError("Please send appropriate document type for profile photo");
                    return resultModel;

                }

               
                file = await _documentService.TryUploadSupportingDocument(vm.File, vm.DocumentType);
                if (file == null)
                {
                    resultModel.AddError("Some files could not be uploaded");

                    return resultModel;
                }
            }

            var user = new User
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                MiddleName = vm.OtherName,
                Email = vm.EmailAddress,
                UserName = vm.EmailAddress,
                PhoneNumber = vm.PhoneNumber,
                UserType = UserType.Parent,
            };


            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            IdentityResult userResult;

            if(existingUser != null)
            {
                existingUser.FirstName = vm.FirstName;
                existingUser.LastName = vm.LastName;
                existingUser.Email = vm.EmailAddress.Trim();
                existingUser.UserName = vm.EmailAddress.Trim();
                existingUser.PhoneNumber = vm.PhoneNumber;
                existingUser.UserType = UserType.Parent;

                userResult = await _userManager.UpdateAsync(existingUser);
            }
            else
            {
                userResult = await _userManager.CreateAsync(user, vm.PhoneNumber);
            }

            if (!userResult.Succeeded)
            {
                resultModel.AddError(string.Join(';', userResult.Errors.Select(x => x.Description)));
                return resultModel;
            }


            var parent = new Parent
            {
                HomeAddress = vm.HomeAddress,
                IdentificationNumber = vm.IdentificationNumber,
                IdentificationType = vm.ModeOfIdentification,
                Occupation = vm.Occupation,
                OfficeAddress = vm.OfficeAddress,
                SecondaryEmail = vm.SecondaryEmailAddress,
                SecondaryPhoneNumber = vm.SecondaryPhoneNumber,
                Sex = vm.Sex,
                Status = vm.Status,
                UserId = user.Id,
                Title= vm.Title,
                
                FileUploads = new List<FileUpload> { file }
            };

            var lastRegNumber = await _parentRepo.GetAll().OrderBy(m => m.Id).Select(m=>m.RegNumber).LastOrDefaultAsync();
            var lastNumber = 0;
            var seperator = schoolProperty.Data.Seperator;
            if (!string.IsNullOrWhiteSpace(lastRegNumber))
            {
                lastNumber = int.Parse(lastRegNumber.Split(seperator).Last());
            }
            var nextNumber = lastNumber;
            var firstTime = true;
            var saved = false;

            while (!saved)
            {
                try
                {
                    nextNumber++;
                    if(firstTime && !string.IsNullOrWhiteSpace(parent.RegNumber))
                    {
                        firstTime = false;
                        _parentRepo.Insert(parent);
                        await _unitOfWork.SaveChangesAsync();
                    }
                    else
                    {
                        parent.RegNumber = $"PAT{seperator}{DateTime.Now.Year}{seperator}{nextNumber.ToString("00000")}";
                        firstTime = false;
                        _parentRepo.Insert(parent);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    saved = true;
                }
                // 2627 is unique constraint (includes primary key), 2601 is unique index
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException && (sqlException.Number == 2627 || sqlException.Number == 2601))
                {
                    saved = false;
                }
            }

            //add usertype to claims
            await _userManager.AddClaimAsync(existingUser ?? user, new System.Security.Claims.Claim(ClaimsKey.UserType, UserType.Parent.GetDescription()));


            //change user's username to reg number
            user.UserName = parent.RegNumber;
            user.NormalizedUserName = parent.RegNumber.ToUpper();
            await _userManager.UpdateAsync(existingUser ?? user);

            _unitOfWork.Commit();


            //broadcast login detail to email
            _ = await _authUserManagementService.SendRegistrationEmail(existingUser ?? user, "");

            //Publish to services
            await _publishService.PublishMessage(Topics.Parent, BusMessageTypes.PARENT, new ParentSharedModel
            {
                Id = parent.Id,
                SecondaryEmail = parent.SecondaryEmail,
                IsActive = true,
                SecondaryPhoneNumber = parent.SecondaryPhoneNumber,
                HomeAddress = parent.HomeAddress,
                UserId = parent.UserId,
                IsDeleted = parent.IsDeleted,
                OfficeAddress = parent.OfficeAddress,
                FirstName = existingUser?.FirstName ?? user.FirstName,
                LastName = existingUser?.LastName ?? user.LastName,
                Email = existingUser?.Email ?? user.Email,
                Phone = existingUser?.PhoneNumber ?? user.PhoneNumber,
                RegNumber = parent.RegNumber
            });

            parent.User = existingUser ?? user;
            var returnModel = (ParentDetailVM)parent;
            resultModel.Data = returnModel;
            return resultModel;
        }

        public async Task<ResultModel<ParentDetailVM>> UpdateParent(long Id, UpdateParentVM vm)
        {
            var resultModel = new ResultModel<ParentDetailVM>();

            _unitOfWork.BeginTransaction();

            var parent = await _parentRepo.GetAll().Include(m=>m.User).FirstOrDefaultAsync(m=>m.Id == Id);

            if (parent == null)
            {
                resultModel.AddError($"No parent for id : {Id}");
                return resultModel;
            }

            var file = new FileUpload();
            //save filles
            if (vm.File != null)
            {
                if (vm.DocumentType != DocumentType.ProfilePhoto)
                {
                    resultModel.AddError("Please send appropriate document type for profile photo");
                    return resultModel;
                }

                file = await _documentService.TryUploadSupportingDocument(vm.File, vm.DocumentType);
                if (file == null)
                {
                    resultModel.AddError("Some files could not be uploaded");
                    return resultModel;
                }

            }


            parent.User.FirstName = vm.FirstName;
            parent.User.LastName = vm.LastName;
            parent.User.MiddleName = vm.OtherName;
            parent.User.Email = vm.EmailAddress;
            parent.User.UserName = vm.EmailAddress.Trim();
            parent.User.NormalizedEmail = vm.EmailAddress.Trim().ToUpper();
            parent.User.UserName = parent.RegNumber;
            parent.User.NormalizedUserName = parent.RegNumber.ToUpper();
            parent.User.PhoneNumber = vm.PhoneNumber;

            parent.HomeAddress = vm.HomeAddress;
            parent.IdentificationNumber = vm.IdentificationNumber;
            parent.IdentificationType = vm.ModeOfIdentification;
            parent.Occupation = vm.Occupation;
            parent.OfficeAddress = vm.OfficeAddress;
            parent.SecondaryEmail = vm.SecondaryEmailAddress;
            parent.SecondaryPhoneNumber = vm.SecondaryPhoneNumber;
            parent.Sex = vm.Sex;
            parent.Status = vm.Status;
            parent.Title = vm.Title;

            if (vm.File != null)
            {
                parent.FileUploads = new List<FileUpload> { file };
            }

            await _parentRepo.UpdateAsync(parent);

            await _unitOfWork.SaveChangesAsync();

            _unitOfWork.Commit();

            //PublishMessage
            await _publishService.PublishMessage(Topics.Parent, BusMessageTypes.PARENT, new ParentSharedModel
            {
                Id = parent.Id,
                SecondaryEmail = parent.SecondaryEmail,
                IsActive = true,
                SecondaryPhoneNumber = parent.SecondaryPhoneNumber,
                HomeAddress = parent.HomeAddress,
                UserId = parent.UserId,
                IsDeleted = parent.IsDeleted,
                OfficeAddress = parent.OfficeAddress,
                FirstName = parent.User.FirstName,
                LastName = parent.User.LastName,
                Email = parent.User.Email,
                Phone = parent.User.PhoneNumber,
                RegNumber = parent.RegNumber
            });

            resultModel.Data = (ParentDetailVM)parent;

            return resultModel;
        }


        public async Task<ResultModel<byte[]>> GetParentExcelSheet()
        {

            var data = new AddParentVM().ToExcel("Parent Excel Sheet");

            if (data == null)
            {
                return new ResultModel<byte[]>("An error occurred while generating excel");
            }
            else
            {
                return new ResultModel<byte[]>(data);
            }
        }
    }
}
