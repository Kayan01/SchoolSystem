using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.AccountType;
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
    public class AccountTypeService : IAccountTypeService
    {
        private readonly IRepository<AccountType, long> _accountTypeRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AccountTypeService(IUnitOfWork unitOfWork, IRepository<AccountType, long> accountTypeRepo)
        {
            _unitOfWork = unitOfWork;
            _accountTypeRepo = accountTypeRepo;
        }

        public async Task<ResultModel<string>> AddAccountType(AccountTypePostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _accountTypeRepo.GetAll().Where(m => m.Name == model.Name.ToUpper()).FirstOrDefaultAsync();

            if (check != null)
            {
                result.AddError("Account type with this name already exist!");
                return result;
            }

            var newAccountType = new AccountType()
            {
                IsActive = model.IsActive,
                AccountClassId = model.AccountClassId,
                Description = model.Description,
                Name = model.Name.ToUpper(),
            };

            _accountTypeRepo.Insert(newAccountType);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<AccountTypeVM>> GetAccountType(long id)
        {
            var result = new ResultModel<AccountTypeVM>();

            result.Data = await _accountTypeRepo.GetAll().Where(m => m.Id == id).Select(m=> new AccountTypeVM() { 
                AccountClass = m.AccountClass.Name,
                AccountClassId = m.AccountClassId,
                IsActive = m.IsActive,
                Description = m.Description,
                Id = m.Id,
                Name = m.Name
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ResultModel<List<AccountTypeVM>>> GetAccountTypes()
        {
            var result = new ResultModel<List<AccountTypeVM>>();

            result.Data = await _accountTypeRepo.GetAll().Select(m => new AccountTypeVM()
            {
                AccountClass = m.AccountClass.Name,
                AccountClassId = m.AccountClassId,
                IsActive = m.IsActive,
                Description = m.Description,
                Id = m.Id,
                Name = m.Name
            }).ToListAsync();

            return result;
        }

        public async Task<ResultModel<List<AccountTypeVM>>> GetAccountTypesByAccountClass(long accountClassId)
        {
            var result = new ResultModel<List<AccountTypeVM>>();

            result.Data = await _accountTypeRepo.GetAll().Where(m => m.AccountClassId == accountClassId).Select(m => new AccountTypeVM()
            {
                AccountClass = m.AccountClass.Name,
                AccountClassId = m.AccountClassId,
                IsActive = m.IsActive,
                Description = m.Description,
                Id = m.Id,
                Name = m.Name
            }).ToListAsync();

            return result;
        }

        public async Task<ResultModel<string>> UpdateAccountType(long Id, AccountTypePostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _accountTypeRepo.GetAll().Where(m => m.Id == Id).FirstOrDefaultAsync();

            if (check == null)
            {
                result.AddError("Account class not found!");
                return result;
            }

            check.IsActive = model.IsActive;
            check.Name = model.Name;
            check.Description = model.Description;
            check.AccountClassId = model.AccountClassId;

            _accountTypeRepo.Update(check);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Update successful";
            return result;
        }

    }
}
