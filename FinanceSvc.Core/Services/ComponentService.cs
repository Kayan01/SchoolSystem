using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.Component;
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
    public class ComponentService : IComponentService
    {
        private readonly IRepository<Component, long> _componentRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ComponentService(IUnitOfWork unitOfWork, IRepository<Component, long> componentRepo)
        {
            _unitOfWork = unitOfWork;
            _componentRepo = componentRepo;
        }

        public async Task<ResultModel<string>> AddComponent(ComponentPostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _componentRepo.GetAll().Where(m => m.Name == model.Name.ToUpper()).FirstOrDefaultAsync();

            if (check != null)
            {
                result.AddError("Component with this name already exist!");
                return result;
            }

            var newComponent = new Component()
            {
                IsActive = model.IsActive,
                AccountId = model.AccountId,
                SequenceNumber = model.SequenceNumber,
                Name = model.Name.ToUpper(),
                Terms = string.Join(',', model.Terms),
            };

            _componentRepo.Insert(newComponent);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<ComponentVM>> GetComponent(long id)
        {
            var result = new ResultModel<ComponentVM>();

            result.Data = await _componentRepo.GetAll().Where(m => m.Id == id).Select(m => new ComponentVM()
            {
                Account = m.Account.Name,
                AccountId = m.AccountId,
                IsActive = m.IsActive,
                Id = m.Id,
                Name = m.Name,
                SequenceNumber = m.SequenceNumber,
                Terms = m.Terms
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ResultModel<List<ComponentVM>>> GetComponents()
        {
            var result = new ResultModel<List<ComponentVM>>();

            result.Data = await _componentRepo.GetAll().Select(m => new ComponentVM()
            {
                Account = m.Account.Name,
                AccountId = m.AccountId,
                IsActive = m.IsActive,
                Id = m.Id,
                Name = m.Name,
                SequenceNumber = m.SequenceNumber,
                Terms = m.Terms
            }).ToListAsync();

            return result;
        }

        public async Task<ResultModel<string>> UpdateComponent(long Id, ComponentPostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _componentRepo.GetAll().Where(m => m.Id == Id).FirstOrDefaultAsync();

            if (check == null)
            {
                result.AddError("Component not found!");
                return result;
            }

            check.IsActive = model.IsActive;
            check.AccountId = model.AccountId;
            check.SequenceNumber = model.SequenceNumber;
            check.Name = model.Name.ToUpper();
            check.Terms = string.Join(',', model.Terms);

            _componentRepo.Update(check);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Update successful";
            return result;
        }
    }
}
