using ProtoBuf.Meta;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
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
using Microsoft.OpenApi.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Auth.Core.Models.Contacts;
using Microsoft.AspNetCore.Mvc;
using IPagedList;
using Shared.Pagination;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;

namespace Auth.Core.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly IDocumentService _documentService;
        private readonly IRepository<School, long> _schoolRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        public SchoolService(
            IRepository<School, long> schoolRepo, 
            IUnitOfWork unitOfWork,
            IDocumentService documentService,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _schoolRepo = schoolRepo;
            _documentService = documentService;
            _userManager = userManager;
        }

        public async Task<ResultModel<SchoolVM>> AddSchool(CreateSchoolVM model)
        {
            var result = new ResultModel<SchoolVM>();
          
            
            _unitOfWork.BeginTransaction();
            var files = new List<FileUpload>();
            //save filles
            if (model.Documents != null && model.Documents.Any())
            {
                 files = _documentService.TryUploadSupportingDocuments(model.Documents);

                if (files.Count() != model.Documents.Count())
                {
                    result.AddError("Some files could not be uploaded");

                    return result;
                }
            }
         

          

            //add schoool admin
            var user = new User
            {
                FirstName = model.ContactFirstName,
                LastName = model.ContactLastName,
                Email = model.ContactEmail,
                UserName = model.ContactEmail,
                PhoneNumber = model.ContactPhoneNo,
                UserType = UserType.Admin,
            };

            var authResult = await _userManager.CreateAsync(user);

            if (!authResult.Succeeded)
            {
                result.AddError(string.Join(';', authResult.Errors.Select(x => x.Description)));
                return result;
            }

            //todo: add more props
            var contactDetails = new SchoolContactDetails
            {
                Email = model.ContactEmail,
                FirstName = model.ContactFirstName,
                LastName = model.ContactLastName,
                PhoneNo = model.ContactPhoneNo
            };
            var school = _schoolRepo.Insert(
                new School
                {
                    Name = model.Name,
                    Address = model.Address,
                    City = model.City,
                    ContactDetails = contactDetails,
                    Country = model.Country,
                    State = model.State,
                    WebsiteAddress = model.WebsiteAddress,
                    FileUploads = files
                });

            await _unitOfWork.SaveChangesAsync();
            model.Id = school.Id;
            result.Data = new SchoolVM { 
             Name = model.Name,
              State = model.State,
  
            
            };
            return result;
        }

        public async Task<ResultModel<bool>> DeleteSchool(long Id)
        {
            var result = new ResultModel<bool>();

            var sch = await _schoolRepo.FirstOrDefaultAsync(Id);
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

        public async Task<ResultModel<PaginatedModel<SchoolVM>>> GetAllSchools(QueryModel model)
        {
            var query = _schoolRepo.GetAll()
                .Select(x => new SchoolVM
                {
                    Id = x.Id,
                    Name = x.Name
                });

            var pagedData = await PaginatedList<SchoolVM>.CreateAsync(query, model.PageIndex, model.PageSize);

            var data = new PaginatedModel<SchoolVM>(pagedData, model.PageIndex, model.PageSize, pagedData.Count);
            var result = new ResultModel<PaginatedModel<SchoolVM>>
            {
                Data = data
            };

            return result;
        }
        public async Task<ResultModel<SchoolVM>> GetSchoolById(long Id)
        {
            var result = new ResultModel<SchoolVM>();
            var school = await _schoolRepo.FirstOrDefaultAsync(x => x.Id == Id);

            if (school == null)
            {
                return result;
            }

            result.Data = school;
            return result;
        }

        public async Task<ResultModel<SchoolUpdateVM>> UpdateSchool(SchoolUpdateVM model)
        {
            var sch = await _schoolRepo.FirstOrDefaultAsync(model.Id);
            var result = new ResultModel<SchoolUpdateVM>();

            if (sch == null)
            {
                result.AddError("School does not exist");
                return result;
            }

            //TODO: add more props
            sch.Name = model.Name;

            await _schoolRepo.UpdateAsync(sch);
            await _unitOfWork.SaveChangesAsync();
            result.Data = model;
            return result;
        }
    }
}