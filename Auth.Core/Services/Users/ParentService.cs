﻿using ArrayToPdf;
using Auth.Core.Interfaces.Setup;
using Auth.Core.Interfaces.Users;
using Auth.Core.Models;
using Auth.Core.Models.Contact;
using Auth.Core.Models.Users;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.Parent;
using Auth.Core.ViewModels.School;
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
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
        private readonly IHttpUserService _httpUserService;
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
            IAuthUserManagement authUserManagementService,
            IHttpUserService httpUserService
            )
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
            _httpUserService = httpUserService;
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

            var query = _parentRepo.GetAll()
                .Include(x => x.User)
                .Include(x => x.Students)
                .Include(x => x.FileUploads);

            var parents = await query.ToPagedListAsync(vm.PageIndex, vm.PageSize);

            var data = new PaginatedModel<ParentListVM>(parents.Select(x => (ParentListVM)x), vm.PageIndex, vm.PageSize, parents.TotalItemCount);

            resultModel.Data = data;

            return resultModel;
        }

        public async Task<ResultModel<PaginatedModel<ParentListDetailVM>>> GetAllParentsInSchool(long schoolId, QueryModel vm)
        {

            var resultModel = new ResultModel<PaginatedModel<ParentListDetailVM>>();

            var query = await _parentRepo.GetAll().Include(x => x.Students)
                .Where(x => x.Students.Any(n => n.TenantId == schoolId))
                .Select(x => new
                {
                    Email = x.User.Email,
                    FullName = x.User.FullName,
                    Id = x.Id,
                    ParentCreationYear = x.CreationTime.Year,
                    PhoneNumber = x.User.PhoneNumber,
                    Status = x.Status,
                    Image = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName()).Path,
                    ChildDetails = x.Students.Where(x => x.TenantId == schoolId).Select(x => new StudentDT
                    {
                        StudentName = x.User.FullName,
                        ClassName = x.Class.FullName
                    }).ToList()
                }).ToListAsync();

            if (query != null)
            {
                var parents = query.Select(x => new ParentListDetailVM
                {
                    Email = x.Email,
                    FullName = x.FullName,
                    ParentCode = $"PRT/{x.ParentCreationYear}/{x.Id}",
                    Id = x.Id,
                    PhoneNumber = x.PhoneNumber,
                    Status = x.Status,
                    Image = x.Image == null ? null : _documentService.TryGetUploadedFile(x.Image),
                    ChildDetails = x.ChildDetails,
                    TotalKidsInSchool = x.ChildDetails.Count
                }).ToPagedList(vm.PageIndex, vm.PageSize);

                var data = new PaginatedModel<ParentListDetailVM>(parents, vm.PageIndex, vm.PageSize, query.Count);
                resultModel.Data = data;

                return resultModel;
            }

            return resultModel;
        }


        public async Task<ResultModel<ParentDetailVM>> GetParentById(long Id)
        {
            var resultModel = new ResultModel<ParentDetailVM>();

            var query = await _parentRepo.GetAll()
                .Where(x => x.Id == Id)
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

            var children = query.Children?.Select(x => new ChildView
            {
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
                .ThenInclude(x => x.User)
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
                schools.Add(new SchoolParentViewModel
                {
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


            
            IdentityResult userResult;


            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
                return new ResultModel<ParentDetailVM>($"User with Email {existingUser.Email} already exist");
            
            userResult = await _userManager.CreateAsync(user, vm.PhoneNumber);

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
                Title = vm.Title,

                FileUploads = new List<FileUpload> { file }
            };

            var lastRegNumber = await _parentRepo.GetAll().OrderBy(m => m.Id).Select(m => m.RegNumber).LastOrDefaultAsync();
            var lastNumber = 0;
            var seperator = schoolProperty.Data.Seperator;
            if (!string.IsNullOrWhiteSpace(lastRegNumber))
            {
                var rtn = int.TryParse(lastRegNumber.Split(seperator).Last(), out var num);

                lastNumber = rtn ? num : 0;
            }
            var nextNumber = lastNumber;
            var firstTime = true;
            var saved = false;

            while (!saved)
            {
                try
                {
                    nextNumber++;
                    if (firstTime && !string.IsNullOrWhiteSpace(parent.RegNumber))
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
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.UserType, UserType.Parent.GetDescription()));


            //change user's username to reg number
            user.UserName = parent.RegNumber;
            user.NormalizedUserName = parent.RegNumber.ToUpper();
            await _userManager.UpdateAsync(user);

            _unitOfWork.Commit();
            var school = await _schoolRepo.GetAll().Where(m => m.Id == schoolProperty.Data.TenantId).Include(x => x.SchoolContactDetails).FirstOrDefaultAsync();
            var contactdetails = school.SchoolContactDetails.Where(m => m.SchoolId == schoolProperty.Data.TenantId).FirstOrDefault();

            //broadcast login detail to email
            _ = await _authUserManagementService.SendRegistrationEmail(user, "", school.Name, contactdetails.Email, school.Address, contactdetails.PhoneNumber, contactdetails.EmailPassword);

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
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                RegNumber = parent.RegNumber
            });

            parent.User = user;
            var returnModel = (ParentDetailVM)parent;
            resultModel.Data = returnModel;
            return resultModel;
        }

        public async Task<ResultModel<ParentDetailVM>> UpdateParent(long Id, UpdateParentVM vm)
        {
            var resultModel = new ResultModel<ParentDetailVM>();

            _unitOfWork.BeginTransaction();

            var parent = await _parentRepo.GetAll().Include(m => m.User).FirstOrDefaultAsync(m => m.Id == Id);

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


        public async Task<ResultModel<bool>> UploadBulkParentData(IFormFile excelfile)
        {
            var resultModel = new ResultModel<bool>();

            var schoolProperty = await _schoolPropertyService.GetSchoolProperty();
            if (schoolProperty.HasError)
            {
                resultModel.AddError(schoolProperty.ValidationErrors);
                return resultModel;
            }

            var importedData = ExcelReader.FromExcel<AddParentVM>(excelfile);

            //Check if imported data contains any data
            if (importedData.Count < 1)
            {
                resultModel.AddError("No data was Imported");

                return resultModel;
            }

            var parents = new List<Parent>();

            _unitOfWork.BeginTransaction();

            foreach (var parent in importedData)
            {
                var user = new User
                {
                    FirstName = parent.FirstName,
                    LastName = parent.LastName, 
                    Email = parent.EmailAddress.Trim(),
                    UserName = parent.EmailAddress.Trim(),
                    PhoneNumber = parent.PhoneNumber,
                    UserType = UserType.Parent
                };

                IdentityResult userResult;

                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                    return new ResultModel<bool>($"User with Email {existingUser.Email} already exist");

                userResult = await _userManager.CreateAsync(user, parent.PhoneNumber);

                if (userResult.Succeeded)
                {
                    resultModel.AddError(string.Join(';', userResult.Errors.Select(x => x.Description)));
                    return resultModel;
                }

                //Add TenantId to UserClaims
              //  await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.TenantId, _httpUserService.GetCurrentUser().TenantId?.ToString()));
                //Add User-Type to claims
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.UserType, UserType.Parent.GetDescription()));

                var ParentUpload = new Parent
                {
                    HomeAddress = parent.HomeAddress,
                    IdentificationNumber = parent.IdentificationNumber,
                    IdentificationType = parent.ModeOfIdentification,
                    Occupation = parent.Occupation,
                    OfficeAddress = parent.OfficeAddress,
                    SecondaryEmail = parent.SecondaryEmailAddress,
                    SecondaryPhoneNumber = parent.SecondaryPhoneNumber,
                    Sex = parent.Sex,
                    Status = parent.Status,
                    UserId = user.Id,
                    Title = parent.Title
                };

                var lastRegNumber = await _parentRepo.GetAll().OrderBy(m => m.Id).Select(m => m.RegNumber).LastOrDefaultAsync();
                var lastNumber = 0;
                var seperator = schoolProperty.Data.Seperator;
                if (!string.IsNullOrWhiteSpace(lastRegNumber))
                {
                    var rtn = int.TryParse(lastRegNumber.Split(seperator).Last(), out var num);

                    lastNumber = rtn ? num : 0;
                }
                var nextNumber = lastNumber;
                var firstTime = true;
                var saved = false;

                while (!saved)
                {
                    try
                    {
                        nextNumber++;
                        if (firstTime && !string.IsNullOrWhiteSpace(ParentUpload.RegNumber))
                        {
                            firstTime = false;
                            _parentRepo.Insert(ParentUpload);
                            await _unitOfWork.SaveChangesAsync();
                        }
                        else
                        {
                            ParentUpload.RegNumber = $"PAT{seperator}{DateTime.Now.Year}{seperator}{nextNumber.ToString("00000")}";
                            firstTime = false;
                            _parentRepo.Insert(ParentUpload);
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

                //change user's username to reg number
                user.UserName = ParentUpload.RegNumber;
                user.NormalizedUserName = ParentUpload.RegNumber.ToUpper();
                await _userManager.UpdateAsync(user);

                _unitOfWork.Commit();

                var school = await _schoolRepo.GetAll().Where(m => m.Id == schoolProperty.Data.TenantId).Include(x => x.SchoolContactDetails).FirstOrDefaultAsync();
                var contactdetails = school.SchoolContactDetails.Where(m => m.SchoolId == schoolProperty.Data.TenantId).FirstOrDefault();

                //broadcast login detail to email
                var emailResult = await _authUserManagementService.SendRegistrationEmail(user, "", school.Name, contactdetails.Email, school.Address, contactdetails.PhoneNumber, contactdetails.EmailPassword);

                if (emailResult.HasError)
                {
                    return new ResultModel<bool>(emailResult.ErrorMessages);
                }
                parents.Add(ParentUpload);
            }

            _unitOfWork.Commit();

            foreach (var parent in parents)
            {
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
                    RegNumber = parent.RegNumber
                });

                resultModel.Data = true;

                return resultModel;
            }

            return resultModel;
        }

        public async Task<ResultModel<PaginatedModel<ParentListVM>>> GetParentByName(QueryModel vm, string FirstName)
        {
            var resultModel = new ResultModel<PaginatedModel<ParentListVM>>();

            var query = await _parentRepo.GetAll()
               .Include(x => x.User)
               .Include(x => x.Students)
               .Include(x => x.FileUploads)
               .Where(x => x.User.FirstName.Contains(FirstName) || x.User.LastName.Contains(FirstName)).ToListAsync();


            if (query == null)
            {
                resultModel.AddError($"No parent with that Name : {FirstName}");
                return resultModel;
            }

            var parents = query.ToPagedList(vm.PageIndex, vm.PageSize);

            var data = new PaginatedModel<ParentListVM>(parents.Select(x => (ParentListVM)x), vm.PageIndex, vm.PageSize, parents.TotalItemCount);


            resultModel.Data = data;

            return resultModel;
        }

        public async Task<ResultModel<PaginatedModel<ParentListVM>>> GetParentBySchoolAndName(QueryModel vm,SearchParentVm model)
        {
            var resultModel = new ResultModel<PaginatedModel<ParentListVM>>();

            var query = await _parentRepo.GetAll().Where(x => x.Students.Any(n => n.TenantId == model.SchoolId))
               .Include(x => x.User)
               .Include(x => x.Students)
               .Include(x => x.FileUploads).ToListAsync();

            query.Where(x => x.User.FirstName.Contains(model.Name) || x.User.LastName.Contains(model.Name));


            if (query == null)
            {
                resultModel.AddError($"No parent with that Name : {model.Name}");
                return resultModel;
            }
            var parents = query.ToPagedList(vm.PageIndex, vm.PageSize);

            var data = new PaginatedModel<ParentListVM>(parents.Select(x => (ParentListVM)x), vm.PageIndex, vm.PageSize, parents.TotalItemCount);
            

            resultModel.Data = data;

            return resultModel;
        }

        public async Task<ResultModel<List<ParentListDetailVM>>> ParentInSchoolData(long schoolId)
        {
            var resultModel = new ResultModel<List<ParentListDetailVM>>();

            var query = await _parentRepo.GetAll().Include(x => x.Students)
                .Where(x => x.Students.Any(n => n.TenantId == schoolId))
                .Select(x => new
                {
                    Email = x.User.Email,
                    FullName = x.User.FullName,
                    Id = x.Id,
                    ParentCreationYear = x.CreationTime.Year,
                    PhoneNumber = x.User.PhoneNumber,
                    Status = x.Status,
                    Address = x.HomeAddress,
                    Image = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName()).Path,
                    Student = x.Students.Where(x => x.TenantId == schoolId).ToList(),
                }).ToListAsync();

          

            if (query != null)
            {
                var studentDataQuery = new List<Student>();
                var parents = new List<ParentListDetailVM>();

                var index = 0;
                var studentCount = 0;
                
                while (query.Count != parents.Count)
                {
                    foreach (var studentDetails in query[index].Student)
                    {
                        var studentData = _studentRepo.GetAll()
                            .Include(x => x.User)
                            .Include(x => x.Class).Where(x => x.Id == studentDetails.Id).FirstOrDefault();

                        studentDataQuery.Add(studentData);
                        studentCount += 1;

                        if (query[index].Student.Count == studentCount)
                        {
                            var parent = new ParentListDetailVM()
                            {
                                Email = query[index].Email,
                                FullName = query[index].FullName,
                                ParentCode = $"PRT/{query[index].ParentCreationYear}/{query[index].Id}",
                                Id = query[index].Id,
                                PhoneNumber = query[index].PhoneNumber,
                                Status = query[index].Status,
                                HomeAddress = query[index].Address,
                                Image = query[index].Image == null ? null : _documentService.TryGetUploadedFile(query[index].Image),
                                Student = studentDataQuery,
                            };
                            
                            parents.Add(parent);
                            index++;
                            studentCount = 0;
                            studentDataQuery = new List<Student>();
                        }
                        
                    }
                }
                resultModel.Data = parents;
              }

            return resultModel;
        }


        public async Task<ResultModel<ExportPayloadVM>> ExportParentDetailsExcel(List<ParentListDetailVM> model)
        {
            var resultModel = new ResultModel<ExportPayloadVM>();

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var workSheet = workbook.Worksheets.Add("AttendanceSheet");

                    for (int i = 1; i <= 7; i++)
                    {
                        var headFormat = workSheet.Cell(1, i);
                        headFormat.Style.Font.SetBold();
                        headFormat.WorksheetRow().Height = 15;
                    }
                    var currentRow = 1;

                    workSheet.Cell(1, 1).Value = "FULL NAME";
                    workSheet.Cell(1, 2).Value = "PHONE NUMBER";
                    workSheet.Cell(1, 3).Value = "EMAIL";
                    workSheet.Cell(1, 4).Value = "STATUS";
                    workSheet.Cell(1, 5).Value = "ADDRESS";
                    workSheet.Cell(1, 6).Value = "NO_OF_KIDS";
                    workSheet.Cell(1, 7).Value = "CHILD_NAME";
                    workSheet.Cell(1, 8).Value = "CLASS";

                    foreach (var data in model)
                    {
                        var parent = await _parentRepo.GetAllIncluding(x => x.User).Where(x => x.Id == data.Id).FirstOrDefaultAsync();

                        currentRow += 1;
                        workSheet.Cell(currentRow, 1).Value = $"{data.FullName}";
                        workSheet.Cell(currentRow, 2).Value = $"{data.PhoneNumber}";
                        workSheet.Cell(currentRow, 3).Value = $"{data.Email}";
                        workSheet.Cell(currentRow, 4).Value = $"{data.Status}";
                        workSheet.Cell(currentRow, 5).Value = $"{data.HomeAddress}";
                        workSheet.Cell(currentRow, 6).Value = $"{data.Student.Count}";

                        foreach(var item in data.Student)
                        {
                            workSheet.Cell(currentRow, 7).Value = $"{item.User.FullName}";
                            workSheet.Cell(currentRow, 8).Value = $"{item.Class.FullName}";
                        }
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

        public async Task<ResultModel<ExportPayloadVM>> ExportParentDetailsPDF(List<ParentListDetailVM> model)
        {
            var resultModel = new ResultModel<ExportPayloadVM>();

            var table = new DataTable("AttendanceReport");

            table.Columns.Add("FULL NAME", typeof(string));
            table.Columns.Add("PHONE NUMBER", typeof(string));
            table.Columns.Add("EMAIL", typeof(string));
            table.Columns.Add("ADDRESS", typeof(string));
            table.Columns.Add("NO_Of_KIDS", typeof(string));
            table.Columns.Add("CHILD_NAME", typeof(string));
            table.Columns.Add("CLASS", typeof(string));

            foreach (var item in model)
            {
                table.Rows.Add(item.FullName, item.PhoneNumber,
                    item.Email,item.HomeAddress,item.Student.Count.ToString());

                foreach(var item2 in item.Student)
                {
                    table.Rows.Add("", "", "", "", "", item2.User.FullName, item2.Class.FullName);
                }
            }

            var pdf = table.ToPdf();

            var payload = new ExportPayloadVM
            {
                FileName = "ParentData",
                Base64String = Convert.ToBase64String(pdf)
            };

            resultModel.Data = payload;

            return resultModel;
        }
    }
}
