using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.BankAccount;
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
    public class BankAccountService : IBankAccountService
    {
        private readonly IRepository<BankAccount, long> _bankAccountRepo;
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountService(IUnitOfWork unitOfWork, IRepository<BankAccount, long> bankAccountRepo)
        {
            _unitOfWork = unitOfWork;
            _bankAccountRepo = bankAccountRepo;
        }

        public async Task<ResultModel<string>> AddBankAccount(BankAccountPostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _bankAccountRepo.GetAll().Where(m => m.AccountNumber == model.AccountNumber).FirstOrDefaultAsync();

            if (check != null)
            {
                result.AddError("Account with this account number already exist!");
                return result;
            }

            var newBankAccount = new BankAccount()
            {
                IsActive = model.IsActive,
                AccountName = model.AccountName,
                Bank = model.Bank,
                AccountNumber = model.AccountNumber,
            };

            _bankAccountRepo.Insert(newBankAccount);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<BankAccountVM>> GetBankAccount(long id)
        {
            var result = new ResultModel<BankAccountVM>();

            result.Data = await _bankAccountRepo.GetAll().Where(m => m.Id == id).Select(m => new BankAccountVM()
            {
                AccountName = m.AccountName,
                AccountNumber = m.AccountNumber,
                IsActive = m.IsActive,
                Bank = m.Bank,
                Id = m.Id,
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ResultModel<List<BankAccountVM>>> GetBankAccounts()
        {
            var result = new ResultModel<List<BankAccountVM>>();

            result.Data = await _bankAccountRepo.GetAll().Select(m => new BankAccountVM()
            {
                AccountName = m.AccountName,
                AccountNumber = m.AccountNumber,
                IsActive = m.IsActive,
                Bank = m.Bank,
                Id = m.Id,
            }).ToListAsync();

            return result;
        }

        public async Task<ResultModel<string>> UpdateBankAccount(long Id, BankAccountPostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _bankAccountRepo.GetAll().Where(m => m.Id == Id).FirstOrDefaultAsync();

            if (check == null)
            {
                result.AddError("Account not found!");
                return result;
            }

            check.IsActive = model.IsActive;
            check.AccountName = model.AccountName;
            check.Bank = model.Bank;
            check.AccountNumber = model.AccountNumber;

            _bankAccountRepo.Update(check);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Update successful";
            return result;
        }
    }
}
