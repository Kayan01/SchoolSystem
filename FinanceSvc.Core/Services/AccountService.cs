using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.Account;
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
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account, long> _accountRepo;
        private readonly IRepository<AccountType, long> _accountTypeRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork, IRepository<Account, long> accountRepo, IRepository<AccountType, long> accountTypeRepo)
        {
            _unitOfWork = unitOfWork;
            _accountRepo = accountRepo;
            _accountTypeRepo = accountTypeRepo;
        }

        public async Task<ResultModel<string>> AddAccount(AccountPostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _accountRepo.GetAll().Where(m => m.Name == model.Name.ToUpper() || m.AccountNumber == model.AccountNumber).FirstOrDefaultAsync();

            if (check != null)
            {
                result.AddError("Account with this Name or Account Number already exist!");
                return result;
            }

            var check2 = await _accountTypeRepo.GetAll().Where(m =>m.Id == model.AccountTypeId && m.AccountClass.MaxNumberValue >= model.AccountNumber && m.AccountClass.MinNumberValue <= model.AccountNumber).FirstOrDefaultAsync();

            if (check2 == null)
            {
                result.AddError("Account Number is out of range of the Account Class!");
                return result;
            }

            var newAccount = new Account()
            {
                IsActive = model.IsActive,
                AccountTypeId = model.AccountTypeId,
                Description = model.Description,
                Name = model.Name.ToUpper(),
                AccountNumber = model.AccountNumber,
                CashPostable = model.CashPostable,
                OpeningBalance = model.OpeningBalance
            };

            _accountRepo.Insert(newAccount);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<AccountVM>> GetAccount(long id)
        {
            var result = new ResultModel<AccountVM>();

            result.Data = await _accountRepo.GetAll().Where(m => m.Id == id).Select(m => new AccountVM()
            {
                AccountType = m.AccountType.Name,
                AccountTypeId = m.AccountTypeId,
                IsActive = m.IsActive,
                Description = m.Description,
                Id = m.Id,
                Name = m.Name,
                AccountNumber = m.AccountNumber,
                CashPostable = m.CashPostable,
                OpeningBalance = m.OpeningBalance,
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ResultModel<List<AccountVM>>> GetAccounts()
        {
            var result = new ResultModel<List<AccountVM>>();

            result.Data = await _accountRepo.GetAll().Select(m => new AccountVM()
            {
                AccountType = m.AccountType.Name,
                AccountTypeId = m.AccountTypeId,
                IsActive = m.IsActive,
                Description = m.Description,
                Id = m.Id,
                Name = m.Name,
                AccountNumber = m.AccountNumber,
                CashPostable = m.CashPostable,
                OpeningBalance = m.OpeningBalance,
            }).ToListAsync();

            return result;
        }

        public async Task<ResultModel<List<AccountVM>>> GetAccountsByAccountClass(long accountTypeId)
        {
            var result = new ResultModel<List<AccountVM>>();

            result.Data = await _accountRepo.GetAll().Where(m => m.AccountTypeId == accountTypeId).Select(m => new AccountVM()
            {
                AccountType = m.AccountType.Name,
                AccountTypeId = m.AccountTypeId,
                IsActive = m.IsActive,
                Description = m.Description,
                Id = m.Id,
                Name = m.Name,
                AccountNumber = m.AccountNumber,
                CashPostable = m.CashPostable,
                OpeningBalance = m.OpeningBalance,
            }).ToListAsync();

            return result;
        }

        public async Task<ResultModel<string>> UpdateAccount(long Id, AccountPostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _accountRepo.GetAll().Where(m => m.Id == Id).FirstOrDefaultAsync();

            if (check == null)
            {
                result.AddError("Account not found!");
                return result;
            }

            check.IsActive = model.IsActive;
            check.AccountTypeId = model.AccountTypeId;
            check.Description = model.Description;
            check.Name = model.Name.ToUpper();
            check.AccountNumber = model.AccountNumber;
            check.CashPostable = model.CashPostable;
            check.OpeningBalance = model.OpeningBalance;

            _accountRepo.Update(check);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Update successful";
            return result;
        }
    }
}
