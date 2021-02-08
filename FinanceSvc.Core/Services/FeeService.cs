using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.Fee;
using FinanceSvc.Core.ViewModels.FeeComponent;
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
    public class FeeService : IFeeService
    {
        private readonly IRepository<Fee, long> _feeRepo;
        private readonly IUnitOfWork _unitOfWork;

        public FeeService(IUnitOfWork unitOfWork, IRepository<Fee, long> feeRepo)
        {
            _unitOfWork = unitOfWork;
            _feeRepo = feeRepo;
        }

        public async Task<ResultModel<string>> AddFee(FeePostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _feeRepo.GetAll().Where(m => m.Name == model.Name.ToUpper()).FirstOrDefaultAsync();

            if (check != null)
            {
                result.AddError("Fee with this name already exist!");
                return result;
            }

            var components = model.FeeComponents.Select(m=> new FeeComponent() { 
                Amount = m.Amount,
                IsCompulsory = m.IsCompulsory,
                ComponentId = m.ComponentId,
            });

            var newFee = new Fee()
            {
                IsActive = model.IsActive,
                Name = model.Name.ToUpper(),
                FeeGroupId = model.FeeGroupId,
                SchoolClassId = model.SchoolClassId,
                SequenceNumber = model.SequenceNumber,
                Terms = model.terms,
                FeeComponents = components.ToList()
            };

            _feeRepo.Insert(newFee);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<FeeWithComponentVM>> GetFee(long id)
        {
            var result = new ResultModel<FeeWithComponentVM>();

            result.Data = await _feeRepo.GetAll().Where(m => m.Id == id).Select(m => new FeeWithComponentVM()
            {
                Id = m.Id,
                Name = m.Name,
                FeeGroupId = m.FeeGroupId,
                SchoolClassId = m.SchoolClassId,
                SequenceNumber = m.SequenceNumber,
                Terms = m.Terms,
                IsActive = m.IsActive,
                Class = $"{m.SchoolClass.Name} {m.SchoolClass.ClassArm}",
                FeeGroup = m.FeeGroup.Name,
                FeeComponents = m.FeeComponents.Select( n=> new FeeComponentVM() { 
                    Amount = n.Amount,
                    Component = n.Component.Name,
                    ComponentId = n.ComponentId,
                    Fee = m.Name,
                    FeeId = m.Id,
                    Id = n.Id,
                    IsCompulsory = n.IsCompulsory,
                }).ToList(),
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ResultModel<List<FeeVM>>> GetFees()
        {
            var result = new ResultModel<List<FeeVM>>();

            result.Data = await _feeRepo.GetAll().Select(m => new FeeVM()
            {
                Id = m.Id,
                Name = m.Name,
                FeeGroupId = m.FeeGroupId,
                SchoolClassId = m.SchoolClassId,
                SequenceNumber = m.SequenceNumber,
                Terms = m.Terms,
                IsActive = m.IsActive,
                Class = $"{m.SchoolClass.Name} {m.SchoolClass.ClassArm}",
                FeeGroup = m.FeeGroup.Name,
            }).ToListAsync();

            return result;
        }

        public async Task<ResultModel<string>> UpdateFee(long Id, FeePostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _feeRepo.GetAll().Where(m => m.Id == Id).FirstOrDefaultAsync();

            if (check == null)
            {
                result.AddError("Fee not found!");
                return result;
            }

            check.IsActive = model.IsActive;
            check.Name = model.Name.ToUpper();

            check.FeeGroupId = model.FeeGroupId;
            check.SchoolClassId = model.SchoolClassId;
            check.SequenceNumber = model.SequenceNumber;
            check.Terms = model.terms;
            check.IsActive = model.IsActive;

            _feeRepo.Update(check);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Update successful";
            return result;
        }
    }
}
