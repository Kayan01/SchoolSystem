using Auth.Core.Models.Users;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Wordprocessing;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Entities;
using Shared.Enums;
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

namespace Auth.Core.Services
{
   public class AdminService :IAdminService
    {
        private readonly IRepository<Admin, long> _adminRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IPublishService _publishService;
        private readonly IDocumentService _documentService;
        public AdminService(
            IRepository<Admin, long> adminRepo,
             IUnitOfWork unitOfWork,
             UserManager<User> userManager,
              IPublishService publishService,
              IDocumentService documentService
            )
        {
            _adminRepo = adminRepo;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _publishService = publishService;
            _documentService = documentService;
        }

        public async Task<ResultModel<AdminVM>> AddAdmin(AddAdminVM model)
        {
            var result = new ResultModel<AdminVM>();

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

            _unitOfWork.BeginTransaction();

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserType = UserType.Admin,
            };
            var userResult = await _userManager.CreateAsync(user, model.Password);

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

            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.Commit();
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

            result.Data = new AdminVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Id = admin.Id
            };

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

        public async Task<ResultModel<AdminVM>> GetAdminById(long Id)
        {
            var result = new ResultModel<AdminVM>();
            var query = _adminRepo.GetAll()
                            .Include(x => x.User)
                            .FirstOrDefault(x => x.UserId == Id);

            result.Data = query;
            return result;
        }

        public async Task<ResultModel<PaginatedModel<AdminVM>>> GetAllAdmin(QueryModel model)
        {
            var result = new ResultModel<PaginatedModel<AdminVM>>();
            var query = _adminRepo.GetAll()
                          .Include(x => x.User);

            var totalCount = query.Count();
            var pagedData = await PaginatedList<Admin>.CreateAsync(query, model.PageIndex, model.PageSize);

            result.Data = new PaginatedModel<AdminVM>(pagedData.Select(x => (AdminVM)x), model.PageIndex, model.PageSize, totalCount);

            return result;
        }

        public Task<ResultModel<AdminVM>> UpdateAdmin(UpdateAdminVM model)
        {
            throw new NotImplementedException();
        }
    }
}
