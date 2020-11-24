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
            //add auth user
            var user = new User
            {
                FirstName = model.ContactFirstName,
                LastName = model.ContactLastName,
                Email = model.ContactEmail,
                UserName = model.ContactEmail,
                PhoneNumber = model.ContactPhoneNo,
                UserType = UserType.Admin,
            };
            var userResult = await _userManager.CreateAsync(user, model.ContactPhoneNo);

            if (!userResult.Succeeded)
            {
                result.AddError(string.Join(';', userResult.Errors.Select(x => x.Description)));
                return result;
            }

            //todo: add more props
            var contactDetails = new SchoolContactDetails  {
                Email = model.ContactEmail,
                FirstName = model.ContactFirstName,
                LastName = model.ContactLastName,
                PhoneNumber = model.ContactPhoneNo,
                IsPrimaryContact = true
            };
            var school =
                new School
                {
                    Name = model.Name,
                    DomainName = model.DomainName,
                    Address = model.Address,
                    City = model.City,
                    Country = model.Country,
                    State = model.State,
                    WebsiteAddress = model.WebsiteAddress,
                    FileUploads = files
                };

            school.SchoolContactDetails.Add(contactDetails);

            _schoolRepo.Insert(school);

            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.Commit();


            result.Data = school;
            return result;
        }

        public async Task<ResultModel<PaginatedModel<SchoolVM>>> GetAllSchools(QueryModel model)
        {
            var query = _schoolRepo.GetAll()
                .Include(x=> x.Staffs)
                .Include(x => x.FileUploads)
                .Include(x => x.Students)
                .Include(x => x.TeachingStaffs);

            var pagedData = await query.ToPagedListAsync(model.PageIndex, model.PageSize);

            var data = new PaginatedModel<SchoolVM>(pagedData.Select(x => (SchoolVM)x), model.PageIndex, model.PageSize, pagedData.TotalItemCount);
            var result = new ResultModel<PaginatedModel<SchoolVM>>
            {
                Data = data
            };

            return result;
        }
        public async Task<ResultModel<SchoolDetailVM>> GetSchoolById(long Id)
        {
            var result = new ResultModel<SchoolDetailVM>();
            var school = await  _schoolRepo
                .GetAll()
                .Where(y => y.Id == Id)
                .Include(h => h.SchoolContactDetails)
                .Include(h => h.FileUploads)
                .Select(x => (SchoolDetailVM)x)                
                .FirstOrDefaultAsync();

            if (school == null)
            {
                return result;
            }

            result.Data = school;
            return result;
        }

        public async Task<ResultModel<SchoolVM>> UpdateSchool(UpdateSchoolVM model, long  id)
        {
            var sch = await _schoolRepo.FirstOrDefaultAsync(id);
            var result = new ResultModel<SchoolVM>();

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

        public async Task<ResultModel<bool>> DeleteSchool(long Id)
        {
            var result = new ResultModel<bool>();

            var sch = await _schoolRepo.GetAll()
                .Include(x => x.SchoolSections)
                .Include(x=> x.Staffs)
                .Include(x => x.Students)
                .Include(x => x.TeachingStaffs)
                .Include(x => x.FileUploads)
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


            _unitOfWork.BeginTransaction();

            foreach (var model in importedData)
            {
                //add admin for school user
                var user = new User
                {
                    FirstName = model.ContactFirstName,
                    LastName = model.ContactLastName,
                    Email = model.ContactEmail,
                    UserName = model.ContactEmail,
                    PhoneNumber = model.ContactPhoneNo,
                    UserType = UserType.Admin,
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
                var school =
                    new School
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
            }

            await _unitOfWork.SaveChangesAsync();

            _unitOfWork.Commit();

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
    }


}