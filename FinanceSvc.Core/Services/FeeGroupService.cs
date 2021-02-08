using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.FeeGroup;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services
{
    public class FeeGroupService : IFeeGroupService
    {
        private readonly IRepository<FeeGroup, long> _feeGroupRepo;
        private readonly IUnitOfWork _unitOfWork;

        public FeeGroupService(IUnitOfWork unitOfWork, IRepository<FeeGroup, long> feeGroupRepo)
        {
            _unitOfWork = unitOfWork;
            _feeGroupRepo = feeGroupRepo;
        }

        public async Task<ResultModel<string>> AddFeeGroup(FeeGroupPostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _feeGroupRepo.GetAll().Where(m => m.Name == model.Name.ToUpper()).FirstOrDefaultAsync();

            if (check != null)
            {
                result.AddError("FeeGroup with this name already exist!");
                return result;
            }

            var newFeeGroup = new FeeGroup()
            {
                IsActive = model.IsActive,
                Description = model.Description,
                Name = model.Name.ToUpper(),
            };

            _feeGroupRepo.Insert(newFeeGroup);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<FeeGroupVM>> GetFeeGroup(long id)
        {
            var result = new ResultModel<FeeGroupVM>();

            result.Data = await _feeGroupRepo.GetAll().Where(m => m.Id == id).Select(m => new FeeGroupVM()
            {
                IsActive = m.IsActive,
                Description = m.Description,
                Id = m.Id,
                Name = m.Name,
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ResultModel<List<FeeGroupVM>>> GetFeeGroups()
        {
            var result = new ResultModel<List<FeeGroupVM>>();

            result.Data = await _feeGroupRepo.GetAll().Select(m => new FeeGroupVM()
            {
                IsActive = m.IsActive,
                Description = m.Description,
                Id = m.Id,
                Name = m.Name,
            }).ToListAsync();

            return result;
        }

        public async Task<ResultModel<string>> UpdateFeeGroup(long Id, FeeGroupPostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _feeGroupRepo.GetAll().Where(m => m.Id == Id).FirstOrDefaultAsync();

            if (check == null)
            {
                result.AddError("FeeGroup not found!");
                return result;
            }

            check.IsActive = model.IsActive;
            check.Description = model.Description;
            check.Name = model.Name.ToUpper();

            _feeGroupRepo.Update(check);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Update successful";
            return result;
        }
    }
}
