using Auth.Core.Models.Users;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using ExcelManager;
using IPagedList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using NPOI.OpenXmlFormats.Wordprocessing;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Entities;
using Shared.Enums;
using Shared.Extensions;
using Shared.FileStorage;
using Shared.Pagination;
using Shared.PubSub;
using Shared.Utils;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Utils.CoreConstants;

namespace Auth.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Admin, long> _adminRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IPublishService _publishService;
        private readonly IDocumentService _documentService;
        private readonly IAuthUserManagement _authUserManagement;
        public AdminService(
            IRepository<Admin, long> adminRepo,
             IUnitOfWork unitOfWork,
             UserManager<User> userManager,
              IPublishService publishService,
              IDocumentService documentService,
              IAuthUserManagement authUserManagement
            )
        {
            _adminRepo = adminRepo;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _publishService = publishService;
            _authUserManagement = authUserManagement;
            _documentService = documentService;
        }

        public async Task<ResultModel<AdminListVM>> AddAdmin(AddAdminVM model)
        {
            var result = new ResultModel<AdminListVM>();


            try
            {
                var files = new List<FileUpload>();
                //save filles
                if (model.Files != null && model.Files.Any())
                {

                    if (model.DocumentTypes == null)
                    {
                        result.AddError("Please send document types for files uploaded");
                        return result;

                    }

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

                _unitOfWork.BeginTransaction();

                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    UserType = UserType.GlobalAdmin,
                };
                var userResult = await _userManager.CreateAsync(user, model.PhoneNumber);

                if (!userResult.Succeeded)
                {
                    result.AddError(string.Join(';', userResult.Errors.Select(x => x.Description)));
                    return result;
                }

                var admin = _adminRepo.Insert(new Admin
                {
                    UserId = user.Id,
                    FileUploads = files

                });



                //add stafftype to claims
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimsKey.UserType, UserType.GlobalAdmin.GetDescription()));

                await _unitOfWork.SaveChangesAsync();


                //broadcast login detail to email
                _ = await _authUserManagement.SendRegistrationEmail(user, "");

                await _publishService.PublishMessage(Topics.Admin, BusMessageTypes.ADMIN, new AdminSharedModel
                {
                    Id = admin.Id,
                    IsActive = true,
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.PhoneNumber,
                });

                result.Data = new AdminListVM
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Id = admin.Id,
                    Image = files.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName())?.Path.GetBase64StringFromImage()
                };

            }
            catch (Exception)
            {
                
                _unitOfWork.Rollback();
                throw;
            }

            _unitOfWork.Commit();
            return result;
        }

        public async Task<ResultModel<bool>> BulkAddAdmin(IFormFile file)
        {
            var result = new ResultModel<bool>();
            var stream = file.OpenReadStream();
            var excelReader = new ExcelReader(stream);

            var importedData = excelReader.ReadAllSheets<AddAdminVM>(false);

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
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    UserType = UserType.GlobalAdmin,
                };

                var userResult = await _userManager.CreateAsync(user, model.PhoneNumber);

                if (!userResult.Succeeded)
                {
                    result.AddError(string.Join(';', userResult.Errors.Select(x => x.Description)));
                    return result;
                }


                //todo: add more props

                var admin = new Admin
                {
                    UserId = user.Id,
                    UserType = UserType.GlobalAdmin
                };

            
                _adminRepo.Insert(admin);
            }

            await _unitOfWork.SaveChangesAsync();

            _unitOfWork.Commit();

            result.Data = true;

            return result;
        }

        public async Task<ResultModel<bool>> DeleteAdmin(long Id)
        {
            //TODO add IS active Status admin
            var result = new ResultModel<bool>();

            var admin = await _adminRepo.GetAllIncluding(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == Id);

            if (admin == null)
            {
                result.AddError($"Admin not found");
                return result;
            }
            //TODO disable teacher

            _unitOfWork.SaveChanges();
            await _publishService.PublishMessage(Topics.Admin, BusMessageTypes.ADMIN_DELETE, new AdminSharedModel
            {
                Id = admin.Id,
                IsActive = false,
                UserId = admin.UserId,
                Email = admin.User.Email,
                FirstName = admin.User.FirstName,
                LastName = admin.User.LastName,
                Phone = admin.User.PhoneNumber,
            });

            result.Data = true;
            return result;
        }

        public async Task<ResultModel<AdminDetailVM>> GetAdminById(long Id)
        {
            var result = new ResultModel<AdminDetailVM>();
            var query = _adminRepo.GetAll()
                            .Include(x => x.User)
                            .Where(x => x.Id == Id)
                            .Include(x=> x.FileUploads)
                            .FirstOrDefault();

            result.Data = query;
            return result;
        }

        public async Task<ResultModel<PaginatedModel<AdminListVM>>> GetAllAdmin(QueryModel model)
        {
            var result = new ResultModel<PaginatedModel<AdminListVM>>();
            var query = _adminRepo.GetAll()
                          .Include(x => x.User)
                          .Include(x => x.FileUploads);



            var admins = await query.ToPagedListAsync(model.PageIndex, model.PageSize);

            result.Data = new PaginatedModel<AdminListVM>(admins.Select(x => (AdminListVM)x), model.PageIndex, model.PageSize, admins.TotalItemCount);

            return result;
        }

        public async Task<ResultModel<AdminListVM>> UpdateAdmin(UpdateAdminVM model)
        {
            var result = new ResultModel<AdminListVM>();
            var admin = await _adminRepo.GetAll()
                          .Include(x => x.User)
                          .Include(x => x.FileUploads)
                          .FirstOrDefaultAsync(x => x.UserId == model.UserId);

            if (admin == null)
            {
                result.AddError("No admin found");

                return result;
            }

            admin.User.Email = model.Email;
            admin.User.FirstName = model.FirstName;
            admin.User.LastName = model.LastName;
            admin.User.PhoneNumber = model.PhoneNumber;
            admin.User.UserName = model.UserName;

            await _adminRepo.UpdateAsync(admin);

            result.Data = admin;

            return result;

        }
    }
}
