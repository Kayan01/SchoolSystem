using Auth.Core.Interfaces.Users;
using Auth.Core.Models;
using Auth.Core.Models.Users;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.Parent;
using IPagedList;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NPOI.Util;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Entities;
using Shared.Enums;
using Shared.FileStorage;
using Shared.Pagination;
using Shared.Utils;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services.Users
{
    public class ParentService : IParentService
    {
        private readonly IRepository<Parent, long> _parentRepo;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IDocumentService _documentService;
        public ParentService(
            IRepository<Parent, long> parentRepo,
            IUnitOfWork unitOfWork,
            IRepository<Student, long> studentRepo,
            UserManager<User> userManager,
            IDocumentService documentService)
        {
            _parentRepo = parentRepo;
            _unitOfWork = unitOfWork;
            _studentRepo = studentRepo;
            _userManager = userManager;
            _documentService = documentService;
        }
        public async Task<ResultModel<ParentDetailVM>> AddNewParent(AddParentVM vm)
        {
            var resultModel = new ResultModel<ParentDetailVM>();

            var file = new FileUpload();
            //save filles
            if (vm.File != null)
            {

                if (vm.DocumentType != DocumentType.ProfilePhoto)
                {
                    resultModel.AddError("Please send appropriate document tye for profile photo");
                    return resultModel;

                }

               
                file = await _documentService.TryUploadSupportingDocument(vm.File, vm.DocumentType);
                if (file == null)
                {
                    resultModel.AddError("Some files could not be uploaded");

                    return resultModel;
                }
            }
            _unitOfWork.BeginTransaction();

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
            var userResult = await _userManager.CreateAsync(user, vm.PhoneNumber);

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

            await   _parentRepo.InsertAsync(parent);

            await  _unitOfWork.SaveChangesAsync();

            _unitOfWork.Commit();

            parent.User = user;
            var returnModel = (ParentDetailVM)parent;
            resultModel.Data = returnModel;
            return resultModel;
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

            var query = _studentRepo.GetAll()
                .Include(x => x.Parent)
                .Select(x => x.Parent);

            var parents = await query.ToPagedListAsync(vm.PageIndex, vm.PageSize);

            var data = new PaginatedModel<ParentListVM>(parents.Select(x => (ParentListVM)x), vm.PageIndex, vm.PageSize, parents.TotalItemCount);

            resultModel.Data = data;

            return resultModel;
        }


        public async Task<ResultModel<ParentDetailVM>> GetParentById(long Id)
        {
            var resultModel = new ResultModel<ParentDetailVM>();

            var parent = await _parentRepo.GetAll()
                .Include(x => x.User)
                .Include(x => x.FileUploads)
                .Where(x=> x.Id == Id)
                .FirstOrDefaultAsync();

            if (parent == null)
            {
                resultModel.AddError($"No parent for id : {Id}");
                return resultModel;
            }

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

        public async Task<ResultModel<ParentDetailVM>> UpdateParent(long Id, UpdateParentVM vm)
        {
            var resultModel = new ResultModel<ParentDetailVM>();

            var parents = await _parentRepo.FirstOrDefaultAsync(Id);

            if (parents == null)
            {
                resultModel.AddError($"No parent for id : {Id}");
                return resultModel;
            }

            //TODO: More props

           await _parentRepo.UpdateAsync(parents);

            await _unitOfWork.SaveChangesAsync();

            resultModel.Data = (ParentDetailVM)parents;

            return resultModel;
        }
    }
}
