﻿using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Core.Models;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Utils;
using Auth.Core.ViewModels.School;
using Shared.Entities;
using Shared.FileStorage;
using Shared.Pagination;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;
using Microsoft.AspNetCore.Http;
using ExcelManager;
using Auth.Core.Models.Contact;
using IPagedList;
using static Shared.Utils.CoreConstants;
using Microsoft.OpenApi.Extensions;
using Shared.PubSub;
using System.Text;
using Shared.Extensions;
using Microsoft.Data.SqlClient;

namespace Auth.Core.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly IDocumentService _documentService;
        private readonly IRepository<School, long> _schoolRepo;
        private readonly IRepository<SchoolGroup, long> _schoolGroupRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IPublishService _publishService;
        private readonly IAuthUserManagement _authUserManagement;
        public SchoolService(
        IRepository<School, long> schoolRepo,
            IUnitOfWork unitOfWork,
         IPublishService publishService,
        IDocumentService documentService,
        IRepository<SchoolGroup, long> schoolGroupRepo,
        IAuthUserManagement authUserManagement,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _schoolRepo = schoolRepo;
            _documentService = documentService;
            _userManager = userManager;
            _publishService = publishService;
            _schoolGroupRepo = schoolGroupRepo;
            _authUserManagement = authUserManagement;
        }

        public async Task<ResultModel<bool>> CheckSchoolDomain(CreateSchoolVM model)
        {
            var domainCheck = await _schoolRepo.GetAll().FirstOrDefaultAsync(x => x.DomainName == model.DomainName);

            if (domainCheck != null)
            {
                return new ResultModel<bool>("Unique name required for domain");
            }
            return new ResultModel<bool>(data : true);
        }

        public async Task<ResultModel<SchoolVM>> AddSchool(CreateSchoolVM model)
        {
            var result = new ResultModel<SchoolVM>();


            var check = await CheckSchoolDomain(model);

            if (check.HasError)
                return new ResultModel<SchoolVM>(check.ErrorMessages);

            _unitOfWork.BeginTransaction();
            var files = new List<FileUpload>();
          
            //use school grouo files for school
            if (model.GroupId.HasValue)
            {
                files = _schoolGroupRepo.GetAll().Include(x => x.FileUploads).Where(x => x.Id == model.GroupId.Value).Select(x => x.FileUploads).FirstOrDefault();
            }


            //save filles
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


            //todo: add more props
            var contactDetails = new SchoolContactDetails
            {
                Email = model.ContactEmail,
                FirstName = model.ContactFirstName,
                LastName = model.ContactLastName,
                PhoneNumber = model.ContactPhoneNo,
                IsPrimaryContact = true
            };

            //get school groupId if it exists


            var school = new School
            {
                Name = model.Name,
                DomainName = model.DomainName.ToLower(),
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                State = model.State,
                WebsiteAddress = model.WebsiteAddress,
                FileUploads = files,
                IsActive = model.IsActive,
                PrimaryColor = model.PrimaryColor,
                SecondaryColor = model.SecondaryColor,
                SchoolGroupId = model.GroupId
            };

            school.SchoolContactDetails.Add(contactDetails);

            _schoolRepo.Insert(school);

            try
            {

                await _unitOfWork.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && (sqlException.Number == 2601 || sqlException.Number == 2627))
                {
                    return new ResultModel<SchoolVM>("Unique name required for domain");
                }
            }


            //add auth user
            var user = new User
            {
                FirstName = model.ContactFirstName,
                LastName = model.ContactLastName,
                Email = model.ContactEmail,
                UserName = model.Username,
                PhoneNumber = model.ContactPhoneNo,
                UserType = UserType.SchoolAdmin,
            };
            var userResult = await _userManager.CreateAsync(user, model.ContactPhoneNo);

            if (!userResult.Succeeded)
            {
                await _schoolRepo.DeleteAsync(school);

                await _unitOfWork.SaveChangesAsync();
                return new ResultModel<SchoolVM>(userResult.Errors.Select(x => x.Description).ToList());
            }



            _unitOfWork.Commit();
            //adds tenant id to school primary contact
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.TenantId, school.Id.ToString()));

            //add stafftype to claims
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.UserType, UserType.SchoolAdmin.GetDescription()));

            //broadcast login detail to email
            var emailResult = await _authUserManagement.SendRegistrationEmail(user, school.DomainName);

            if (emailResult.HasError)
            {
                var sb = new StringBuilder();
                _ = emailResult.ErrorMessages.Select(x => { sb.AppendLine(x); return x; }).ToList();

                result.Message = sb.ToString();
            }

            //Publish to services
            await _publishService.PublishMessage(Topics.School, BusMessageTypes.SCHOOL, new SchoolSharedModel
            {
                Id = school.Id,
                IsActive = school.IsActive,
                Email = contactDetails.Email,
                PhoneNumber = contactDetails.PhoneNumber,
                Address = school.Address,
                WebsiteAddress = school.WebsiteAddress,
                City = school.City,
                DomainName = school.DomainName,
                Name = school.Name,
                State = school.State,
                Logo = school.FileUploads.FirstOrDefault(x => x.Name == DocumentType.Logo.GetDisplayName())?.Path
            });


            result.Data = school;
            return result;
        }

        public async Task<ResultModel<PaginatedModel<SchoolVM>>> GetAllSchools(QueryModel model, long? groupId = null)
        {
            var firstQuery = _schoolRepo.GetAll();

            //add where clause to filter schools
            if (groupId != null)
            {
                firstQuery = firstQuery.Where(x => x.SchoolGroupId == groupId);
            }
           var query = firstQuery.Include(x => x.Staffs)
                .Include(x => x.FileUploads)
                .Include(x => x.Students)
                .Include(x => x.TeachingStaffs)
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new
                {
                    x.Address,
                    x.City,
                    x.ClientCode,
                    x.Country,
                    x.CreationTime,
                    x.DomainName,
                    logoPath = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.Logo.GetDisplayName()).Path,
                    x.Id,
                    x.Name,
                    x.SchoolSections,
                    x.State,
                    staffCount = x.Staffs.Count,
                    studentCount = x.Students.Count,
                    teacherCount = x.Students.Count,
                    x.WebsiteAddress,
                    x.IsActive,
                    x.SchoolGroupId
                });


            var pagedData = await query.ToPagedListAsync(model.PageIndex, model.PageSize);

            var data = new PaginatedModel<SchoolVM>(pagedData.Select(x => new SchoolVM
            {
                Id = x.Id,
                Name = x.Name,
                State = x.State,
                Logo = x.logoPath == null ? null : _documentService.TryGetUploadedFile(x.logoPath),
                ClientCode = x.ClientCode,
                DateCreated = x.CreationTime,
                Status = x.IsActive,
                UsersCount = x.staffCount + x.studentCount + x.teacherCount,
                SchoolGroupId = x.SchoolGroupId

            }), model.PageIndex, model.PageSize, pagedData.TotalItemCount);


            var result = new ResultModel<PaginatedModel<SchoolVM>>
            {
                Data = data
            };

            return result;
        }

        public async Task<ResultModel<SchoolNameAndLogoVM>> GetSchoolNameAndLogoById(long Id)
        {
            string logo = default;

            var schoolInfo = await _schoolRepo
                .GetAll()
                .Where(y => y.Id == Id)
                .Select(x => new
                {
                    path = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.Logo.GetDisplayName()).Path,
                    name = x.Name,
                    x.PrimaryColor,
                    x.SecondaryColor
                }).FirstOrDefaultAsync();

            if (schoolInfo is null)
            {
                return new ResultModel<SchoolNameAndLogoVM>(errorMessage: "School not found.");
            }

            if(!(schoolInfo.path == null))
            {
                logo = _documentService.TryGetUploadedFile(schoolInfo.path);
            }
            
            if (string.IsNullOrWhiteSpace(logo))
            {
                return new ResultModel<SchoolNameAndLogoVM>(errorMessage: "Logo not found.");
            }

            return new ResultModel<SchoolNameAndLogoVM>(data: new SchoolNameAndLogoVM()
            {
                SchoolName = schoolInfo.name,
                Logo = logo,
                PrimaryColor = schoolInfo.PrimaryColor,
                SecondaryColor = schoolInfo.SecondaryColor
            });
        }

        public async Task<ResultModel<SchoolNameAndLogoVM>> GetSchoolNameAndLogoByDomain(string domain)
        {

            string logo = default;
            var schoolInfo = await _schoolRepo
                .GetAll()
                .Where(y => y.DomainName == domain.ToLower())
                .Select(x => new
                {
                    path = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.Logo.GetDisplayName()).Path,
                    name = x.Name,
                    x.PrimaryColor,
                    x.SecondaryColor
                }).FirstOrDefaultAsync();

            if (schoolInfo is null)
            {
                return new ResultModel<SchoolNameAndLogoVM>(errorMessage: "School not found.");
            }

            if(!(schoolInfo.path == null))
            {
                logo = _documentService.TryGetUploadedFile(schoolInfo.path);
            }
            
            if (string.IsNullOrWhiteSpace(logo))
            {
                return new ResultModel<SchoolNameAndLogoVM>(errorMessage: "Logo not found.");
            }

            return new ResultModel<SchoolNameAndLogoVM>(data: new SchoolNameAndLogoVM()
            {
                SchoolName = schoolInfo.name,
                Logo = logo,
                PrimaryColor = schoolInfo.PrimaryColor,
                SecondaryColor = schoolInfo.SecondaryColor
            });
        }

        public async Task<ResultModel<SchoolDetailVM>> GetSchoolById(long Id)
        {
            var school = await _schoolRepo
                .GetAll()
                .Where(y => y.Id == Id)
                .Include(h => h.SchoolContactDetails)
                .Include(h => h.FileUploads)
                .Select(x => new
                {
                    x.Address,
                    x.City,
                    x.ClientCode,
                    x.Country,
                    x.DomainName,
                    x.Id,
                    x.IsActive,
                    x.Name,
                    x.State,
                    staffCount = x.Staffs.Count,
                    studentCount = x.Students.Count,
                    teachingStaffCount = x.TeachingStaffs.Count,
                    x.WebsiteAddress,
                    contactDetails = x.SchoolContactDetails.FirstOrDefault(x => x.IsPrimaryContact),
                    logo = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.Logo.GetDisplayName()),
                    icon = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.Icon.GetDisplayName()),
                    x.CreationTime,
                    x.PrimaryColor,
                    x.SecondaryColor
                })
                .FirstOrDefaultAsync();

            if (school == null)
            {
                return new ResultModel<SchoolDetailVM>("No school found");
            }

            return new ResultModel<SchoolDetailVM>
            {
                Data = new SchoolDetailVM
                {
                    WebsiteAddress = school.WebsiteAddress,
                    Address = school.Address,
                    City = school.City,
                    ClientCode = school.ClientCode,
                    ContactEmail = school.contactDetails.Email,
                    ContactFirstName = school.contactDetails.FirstName,
                    ContactLastName = school.contactDetails.LastName,
                    ContactPhone = school.contactDetails.PhoneNumber,
                    Country = school.Country,
                    DateCreated = school.CreationTime,
                    DomainName = school.DomainName,
                    Icon = school.icon == null ? null : _documentService.TryGetUploadedFile(school.icon.Path),
                    Id = school.Id,
                    Logo = school.logo == null ? null : _documentService.TryGetUploadedFile(school.logo.Path),
                    Name = school.Name,
                    StaffCount = school.staffCount,
                    State = school.State,
                    Status = school.IsActive,
                    StudentsCount = school.studentCount,
                    TeachersCount = school.teachingStaffCount,
                    TotalUsersCount = school.teachingStaffCount + school.staffCount + school.studentCount,
                    PrimaryColor = school.PrimaryColor,
                    SecondaryColor = school.SecondaryColor
                }
            };
        }

        public async Task<ResultModel<SchoolVM>> UpdateSchool(UpdateSchoolVM model, long id)
        {


            var domainCheck = await _schoolRepo.GetAll().FirstOrDefaultAsync(x => x.DomainName == model.DomainName);

            if (domainCheck != null)
            {
                return new ResultModel<SchoolVM>("Unique name required for domain");
            }


            var sch = await _schoolRepo.GetAll()
                .Include(x => x.FileUploads)
                .Include(x => x.SchoolContactDetails)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            var result = new ResultModel<SchoolVM>();

            if (sch == null)
            {
                result.AddError("School does not exist");
                return result;
            }


            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return new ResultModel<SchoolVM>("User does not exist");
            }



            _unitOfWork.BeginTransaction();


            var files = new List<FileUpload>();
            //save filles
            if (model.Files != null && model.Files.Any())
            {
                if (model.Files.Count != model.DocumentTypes.Count)
                {
                    result.AddError("Some document types are missing");
                    return result;
                }
                files = await _documentService.TryUploadSupportingDocuments(model.Files, model.DocumentTypes);
                if (files.Count != model.Files.Count)
                {
                    result.AddError("Some files could not be uploaded");

                    return result;
                }
            }

            var contactDetails = new SchoolContactDetails
            {
                Email = model.ContactEmail,
                FirstName = model.ContactFirstName,
                LastName = model.ContactLastName,
                PhoneNumber = model.ContactPhoneNo,
                IsPrimaryContact = true
            };

            sch.Name = model.Name;
            sch.DomainName = model.DomainName;
            sch.Address = model.Address;
            sch.City = model.City;
            sch.Country = model.Country;
            sch.State = model.State;
            sch.WebsiteAddress = model.WebsiteAddress;
            sch.FileUploads = files;
            sch.IsActive = model.IsActive;
            sch.SchoolContactDetails = new List<SchoolContactDetails> { contactDetails };
            sch.FileUploads = files;
            sch.SecondaryColor = model.SecondaryColor;
            sch.PrimaryColor = model.PrimaryColor;

            await _schoolRepo.UpdateAsync(sch);

            try
            {

                await _unitOfWork.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && (sqlException.Number == 2601 || sqlException.Number == 2627))
                {
                    return new ResultModel<SchoolVM>("Unique name required for domain");
                }
            }


            //update auth user

            user.FirstName = model.ContactFirstName;
            user.LastName = model.ContactLastName;
            user.Email = model.ContactEmail;
            user.UserName = model.Username;
            user.PhoneNumber = model.ContactPhoneNo;
            user.UserType = UserType.SchoolAdmin;

            var userResult = await _userManager.UpdateAsync(user);

            if (!userResult.Succeeded)
            {
                return new ResultModel<SchoolVM>(userResult.Errors.Select(x => x.Description).ToList());
            }

            _unitOfWork.Commit();

            //Publish to services
            await _publishService.PublishMessage(Topics.School, BusMessageTypes.SCHOOL, new SchoolSharedModel
            {
                Id = sch.Id,
                IsActive = sch.IsActive,
                Email = contactDetails.Email,
                PhoneNumber = contactDetails.PhoneNumber,
                Address = sch.Address,
                WebsiteAddress = sch.WebsiteAddress,
                City = sch.City,
                DomainName = sch.DomainName,
                Name = sch.Name,
                State = sch.State,
                Logo = sch.FileUploads.FirstOrDefault(x => x.Name == DocumentType.Logo.GetDisplayName())?.Path
            });

            result.Data = sch;
            return result;
        }

        public async Task<ResultModel<bool>> DeleteSchool(long Id)
        {
            var result = new ResultModel<bool>();

            var sch = await _schoolRepo.GetAll()
                .Include(x => x.SchoolSections)
                .Include(x => x.Staffs)
                .ThenInclude(x => x.User)
                .Include(x => x.Students)
                .ThenInclude(x => x.User)
                .Include(x => x.TeachingStaffs)
                .Include(x => x.FileUploads)
                .Include(x => x.SchoolContactDetails)
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();


            if (sch == null)
            {
                result.AddError("School does not exist");
                result.Data = false;
                return result;
            }



            await _schoolRepo.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            result.Data = true;

            return result;
        }

        public async Task<ResultModel<bool>> AddBulkSchool(IFormFile excelfile)
        {
            var result = new ResultModel<bool>();
            var stream = excelfile.OpenReadStream();
            var excelReader = new ExcelReader(stream);

            var importedData = excelReader.ReadAllSheets<CreateSchoolVM>(false);

            //check if imported data contains any data
            if (importedData.Count < 1)
            {
                result.AddError("No data was imported");

                return result;
            }

            var schools = new List<School>();

            _unitOfWork.BeginTransaction();

            foreach (var model in importedData)
            {
                //add admin for school user
                var user = new User
                {
                    FirstName = model.ContactFirstName,
                    LastName = model.ContactLastName,
                    Email = model.ContactEmail,
                    UserName = model.Username,
                    PhoneNumber = model.ContactPhoneNo,
                    UserType = UserType.SchoolAdmin,
                };
                var userResult = await _userManager.CreateAsync(user, model.ContactPhoneNo);

                if (!userResult.Succeeded)
                {
                    result.AddError(string.Join(';', userResult.Errors.Select(x => x.Description)));
                    return result;
                }

                //todo: add more props
                var contactDetails = new SchoolContactDetails
                {
                    Email = model.ContactEmail,
                    FirstName = model.ContactFirstName,
                    LastName = model.ContactLastName,
                    PhoneNumber = model.ContactPhoneNo,
                    IsPrimaryContact = true
                };
                var school = new School
                {
                    Name = model.Name,
                    DomainName = model.DomainName,
                    Address = model.Address,
                    City = model.City,
                    Country = model.Country,
                    State = model.State,
                    WebsiteAddress = model.WebsiteAddress
                };

                school.SchoolContactDetails.Add(contactDetails);

                _schoolRepo.Insert(school);

                schools.Add(school);
            }

            await _unitOfWork.SaveChangesAsync();

            _unitOfWork.Commit();

            foreach (var school in schools)
            {
                //Publish to services
                await _publishService.PublishMessage(Topics.School, BusMessageTypes.SCHOOL, new SchoolSharedModel
                {
                    Id = school.Id,
                    IsActive = school.IsActive,
                    Email = school.SchoolContactDetails[0].Email,
                    PhoneNumber = school.SchoolContactDetails[0].PhoneNumber,
                    Address = school.Address,
                    WebsiteAddress = school.WebsiteAddress,
                    City = school.City,
                    DomainName = school.DomainName,
                    Name = school.Name,
                    State = school.State,
                    Logo = school.FileUploads.FirstOrDefault(x => x.Name == DocumentType.Logo.GetDisplayName()).Path
                });
            }

            result.Data = true;

            return result;
        }

        public async Task<ResultModel<int>> GetTotalSchoolsCount()
        {

            var result = new ResultModel<int>();


            var schoolsCount = await _schoolRepo.CountAsync();

            result.Data = schoolsCount;

            return result;

        }

        public async Task<ResultModel<byte[]>> GetSchoolExcelSheet()
        {

            var data = new CreateSchoolVM().ToExcel("School");

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