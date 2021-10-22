using Shared.DataAccess.EfCore.UnitOfWork;
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
using Auth.Core.ViewModels.SchoolGroup;

namespace Auth.Core.Services
{
    public class SchoolGroupService : ISchoolGroupService
    {
        private readonly IDocumentService _documentService;
        private readonly IRepository<SchoolGroup, long> _schGroupRepo;
        private readonly ISchoolService _schoolService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IPublishService _publishService;
        private readonly IAuthUserManagement _authUserManagement;
        public SchoolGroupService(
        IRepository<SchoolGroup, long> schGroupRepo,
            IUnitOfWork unitOfWork,
         IPublishService publishService,
        IDocumentService documentService,
        IAuthUserManagement authUserManagement,
        ISchoolService schoolService,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _schGroupRepo = schGroupRepo;
            _documentService = documentService;
            _userManager = userManager;
            _publishService = publishService;
            _authUserManagement = authUserManagement;
            _schoolService = schoolService;

        }

        public Task<ResultModel<bool>> AddBulkSchoolGroup(IFormFile model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResultModel<SchoolGroupListVM>> AddSchoolGroup(CreateSchoolGroupVM model)
        {
            var result = new ResultModel<SchoolGroupListVM>();
            _unitOfWork.BeginTransaction();

            var files = new List<FileUpload>();
            //save filles
            if (model.Files != null && model.Files.Any())
            {
                if (model.Files.Count != model.DocumentTypes.Count)
                {

                    return new ResultModel<SchoolGroupListVM>("Some document types are missing");
                }
                files = await _documentService.TryUploadSupportingDocuments(model.Files, model.DocumentTypes);
                if (files.Count() != model.Files.Count())
                {
                    return new ResultModel<SchoolGroupListVM>("Some files could not be uploaded");
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

            var schGroup = new SchoolGroup()
            {
                Name = model.Name,
                WebsiteAddress = model.WebsiteAddress,
                FileUploads = files,
                IsActive = model.IsActive,
                PrimaryColor = model.PrimaryColor,
                SecondaryColor = model.SecondaryColor,

            };

            schGroup.SchoolContactDetails.Add(contactDetails);
            _schGroupRepo.Insert(schGroup);

            //add auth user
            var user = new User
            {
                FirstName = model.ContactFirstName,
                LastName = model.ContactLastName,
                Email = model.ContactEmail,
                UserName = model.ContactEmail,
                PhoneNumber = model.ContactPhoneNo,
                UserType = UserType.SchoolGroupManager,
            };

            var userResult = await _userManager.CreateAsync(user, model.ContactPhoneNo);

            if (!userResult.Succeeded)
            {
                await _schGroupRepo.DeleteAsync(schGroup);

                await _unitOfWork.SaveChangesAsync();
                return new ResultModel<SchoolGroupListVM>(userResult.Errors.Select(x => x.Description).ToList());
            }


            _unitOfWork.Commit();
            //adds schoolgroup id to school group primary contact
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.SchGroupId, schGroup.Id.ToString()));

            //add stafftype to claims
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.UserType, UserType.SchoolGroupManager.GetDescription()));

            //broadcast login detail to email
             var emailResult = await _authUserManagement.SendRegistrationEmail(user,"");

            if (emailResult.HasError)
            {
                var sb = new StringBuilder();
                _ = emailResult.ErrorMessages.Select(x => { sb.AppendLine(x); return x; }).ToList();

                result.Message = sb.ToString();
            }

            result.Data = schGroup;
            return result;

        }

        public async Task<ResultModel<PaginatedModel<SchoolGroupListVM>>> GetAllSchoolGroups(QueryModel model, long? id)
        {
            var query = _schGroupRepo.GetAll();
            if (id != null)
            {
                query = query.Where(x => x.Id == id);
            }

           var data = await  query.Select(x => new SchoolGroupListVM
            {
                ContactEmail = x.SchoolContactDetails.FirstOrDefault().Email,
                ContactFirstName = x.SchoolContactDetails.FirstOrDefault().FirstName,
                ContactLastName = x.SchoolContactDetails.FirstOrDefault().LastName,
                ContactPhone = x.SchoolContactDetails.FirstOrDefault().LastName,
                DateCreated = x.CreationTime,
                Id = x.Id,
                IsActive = x.IsActive,
                Name = x.Name,
                PrimaryColor = x.PrimaryColor,
                SecondaryColor = x.SecondaryColor,
                WebsiteAddress = x.WebsiteAddress
            }).ToPagedListAsync(model.PageIndex, model.PageSize);

            return new ResultModel<PaginatedModel<SchoolGroupListVM>>(new PaginatedModel<SchoolGroupListVM>(data.Items, model.PageIndex, model.PageSize, data.TotalItemCount));


        }

        public async Task<ResultModel<GetSchoolGroupAnalyticsVM>> GetSchoolGroupAnalytics(long id)
        {
            /*no of schools
             no of students 
             */
            var data = await _schGroupRepo.GetAll().Where(x=> x.Id == id).Select(x => new GetSchoolGroupAnalyticsVM
            {
                NoOfSchools = x.Schools.Count(),
                StudentCounts = x.Schools.Select(x => x.Students.Count())
            }).FirstOrDefaultAsync();

            return new ResultModel<GetSchoolGroupAnalyticsVM>(data);

        }

        public async Task<ResultModel<PaginatedModel<SchoolVM>>> GetAllSchoolInGroup(QueryModel qm, long id)
        {
            return await _schoolService.GetAllSchools(qm, id);
        }

        public async Task<ResultModel<SchoolGroupListVM>> UpdateSchoolGroup(UpdateSchoolGroupVM model, long Id)
        {
            var result = new ResultModel<SchoolGroupListVM>();

            var schgrp = await _schGroupRepo.GetAll()
                .Include(x=> x.SchoolContactDetails)
                .FirstOrDefaultAsync(x => x.Id == Id);

            var user = await _userManager.FindByNameAsync(model.ContactEmail);

            if (schgrp is null)
            {
                return new ResultModel<SchoolGroupListVM>($"No school group for id {Id}");
            }

            if (user is null)
            {
                return new ResultModel<SchoolGroupListVM>("No School group admin found");
            }

            _unitOfWork.BeginTransaction();

            var files = new List<FileUpload>();
            //save filles
            if (model.Files != null && model.Files.Any())
            {
                if (model.Files.Count != model.DocumentTypes.Count)
                {

                    return new ResultModel<SchoolGroupListVM>("Some document types are missing");
                }
                files = await _documentService.TryUploadSupportingDocuments(model.Files, model.DocumentTypes);
                if (files.Count() != model.Files.Count())
                {
                    return new ResultModel<SchoolGroupListVM>("Some files could not be uploaded");
                }
            }

            schgrp.SchoolContactDetails = new List<SchoolContactDetails>{ new SchoolContactDetails
            {
                Email = model.ContactEmail,
                FirstName = model.ContactFirstName,
                LastName = model.ContactLastName,
                PhoneNumber = model.ContactPhoneNo,
                IsPrimaryContact = true
            } };


            schgrp.Name = model.Name;
            schgrp.WebsiteAddress = model.WebsiteAddress;
            schgrp.FileUploads = files;
            schgrp.IsActive = model.IsActive;
            schgrp.PrimaryColor = model.PrimaryColor;
            schgrp.SecondaryColor = model.SecondaryColor;


           await _schGroupRepo.UpdateAsync(schgrp);

            //update auth user

            user.FirstName = model.ContactFirstName;
            user.LastName = model.ContactLastName;
            user.Email = model.ContactEmail;
            user.PhoneNumber = model.ContactPhoneNo;
            user.UserType = UserType.SchoolGroupManager;


            var userResult = await _userManager.UpdateAsync(user);

            if (!userResult.Succeeded)
            {
                await _schGroupRepo.DeleteAsync(schgrp);

                await _unitOfWork.SaveChangesAsync();
                return new ResultModel<SchoolGroupListVM>(userResult.Errors.Select(x => x.Description).ToList());
            }


            _unitOfWork.Commit();          

            result.Data = schgrp;
            return result;
        }
    }


}