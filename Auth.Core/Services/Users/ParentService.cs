using Auth.Core.Interfaces.Users;
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
        private readonly IUnitOfWork _unitOfWork;
        public ParentService(IRepository<Parent, long> parentRepo, IUnitOfWork unitOfWork)
        {
            _parentRepo = parentRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<string>> AddNewParent(AddParentVM vm)
        {
            var resultModel = new ResultModel<string>();

            var parent = new Parent
            {
                StudentId = vm.StudentId,
            };

            await   _parentRepo.InsertAsync(parent);

            await  _unitOfWork.SaveChangesAsync();

            resultModel.Data = "Success";
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

            var parents = await _parentRepo.GetAll()
                .Where(x => x.StudentId == studId)
                .Select(x => (ParentVM)x)
                .ToListAsync();

            if (parents.Count < 1)
            {
                resultModel.AddError($"No parent for student id : {studId}");
                return resultModel;
            }

            resultModel.Data = parents;

            return resultModel;
        }

        public async Task<ResultModel<string>> UpdateParent(long Id, UpdateParentVM vm)
        {
            var resultModel = new ResultModel<string>();

            var parents = await _parentRepo.FirstOrDefaultAsync(Id);

            if (parents == null)
            {
                resultModel.AddError($"No parent for id : {Id}");
                return resultModel;
            }

            //TODO: More props

           await _parentRepo.UpdateAsync(parents);

            await _unitOfWork.SaveChangesAsync();

            resultModel.Data = "Updated";

            return resultModel;
        }
    }
}
