using Auth.Core.Interfaces.Users;
using Auth.Core.Models;
using Auth.Core.Models.Users;
using Auth.Core.ViewModels.Parent;
using IPagedList;
using Microsoft.EntityFrameworkCore;
using NPOI.Util;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
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
        public ParentService(
            IRepository<Parent, long> parentRepo,
            IUnitOfWork unitOfWork,
            IRepository<Student, long> studentRepo)
        {
            _parentRepo = parentRepo;
            _unitOfWork = unitOfWork;
            _studentRepo = studentRepo;
        }
        public async Task<ResultModel<ParentVM>> AddNewParent(AddParentVM vm)
        {
            var resultModel = new ResultModel<ParentVM>();

            var parent = new Parent
            {
                //StudentId = vm.StudentId,
            };

            await   _parentRepo.InsertAsync(parent);

            await  _unitOfWork.SaveChangesAsync();

            resultModel.Data = (ParentVM)parent;
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

        public async Task<ResultModel<IPagedList<ParentVM>>> GetAllParents(PagingVM vm)
        {

            var resultModel = new ResultModel<IPagedList<ParentVM>>();

            var data = await _parentRepo.GetAll().Select(p => new ParentVM
            {

            }).ToPagedListAsync(vm.PageNumber, vm.PageSize);

            resultModel.Data = data;

            return resultModel;
        }

        public async Task<ResultModel<ParentVM>> GetParentById(long Id)
        {
            var resultModel = new ResultModel<ParentVM>();

            var parent = await _parentRepo.FirstOrDefaultAsync(Id);

            if (parent == null)
            {
                resultModel.AddError($"No parent for id : {Id}");
                return resultModel;
            }

            resultModel.Data = (ParentVM)parent;

            return resultModel;
        }

        public async Task<ResultModel<List<ParentVM>>> GetParentsForStudent(long studId)
        {
            var resultModel = new ResultModel<List<ParentVM>>();

            var parents = await _studentRepo.GetAll()
                .Include(x => x.Parent)
                .ThenInclude(x => x.User)
                .Where(x => x.UserId == studId)
                .Select(x => new ParentVM
                {
                      Name = x.Parent.User.FullName,
                       Address = x.Parent.Address
                })
                .ToListAsync();


            if (parents.Count < 1)
            {
                resultModel.AddError($"No parent for student id : {studId}");
                return resultModel;
            }

            resultModel.Data = parents;

            return resultModel;
        }

        public async Task<ResultModel<ParentVM>> UpdateParent(long Id, UpdateParentVM vm)
        {
            var resultModel = new ResultModel<ParentVM>();

            var parents = await _parentRepo.FirstOrDefaultAsync(Id);

            if (parents == null)
            {
                resultModel.AddError($"No parent for id : {Id}");
                return resultModel;
            }

            //TODO: More props

           await _parentRepo.UpdateAsync(parents);

            await _unitOfWork.SaveChangesAsync();

            resultModel.Data = (ParentVM)parents;

            return resultModel;
        }
    }
}
