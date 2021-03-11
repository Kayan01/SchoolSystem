using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels;
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
    public class AccountClassService : IAccountClassService
    {
        private readonly IRepository<AccountClass, long> _accountClassRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AccountClassService(IUnitOfWork unitOfWork, IRepository<AccountClass, long> accountClassRepo)
        {
            _unitOfWork = unitOfWork;
            _accountClassRepo = accountClassRepo;
        }

        public async Task<ResultModel<string>> AddAccountClass(AccountClassVM model)
        {
            var result = new ResultModel<string>();

            var check = await _accountClassRepo.GetAll().Where(m=> m.Name == model.Name.ToUpper()).FirstOrDefaultAsync();

            if (check != null)
            {
                result.AddError("Account type with this name already exist!");
                return result;
            }

            var newAccountClass = new AccountClass()
            {
                IsActive = model.IsActive,
                MaxNumberValue = model.MaxNumberValue,
                MinNumberValue = model.MinNumberValue,
                Name = model.Name.ToUpper()
            };

            _accountClassRepo.Insert(newAccountClass);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<AccountClassVM>> GetAccountClass(long id)
        {
            var result = new ResultModel<AccountClassVM>();

            result.Data = await _accountClassRepo.GetAll().Where(m => m.Id == id).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ResultModel<List<AccountClassVM>>> GetAccountClasses()
        {
            var result = new ResultModel<List<AccountClassVM>>();

            result.Data = await _accountClassRepo.GetAll().Select(m=> (AccountClassVM)m).ToListAsync();

            return result;
        }

        public async Task<ResultModel<string>> UpdateAccountClass(AccountClassVM model)
        {
            var result = new ResultModel<string>();

            var check = await _accountClassRepo.GetAll().Where(m => m.Id == model.Id).FirstOrDefaultAsync();

            if (check == null)
            {
                result.AddError("Account type not found!");
                return result;
            }

            check.Name = model.Name.ToUpper();
            check.IsActive = model.IsActive;
            check.MaxNumberValue = model.MaxNumberValue;
            check.MinNumberValue = model.MinNumberValue;

            _accountClassRepo.Update(check);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Update successful";
            return result;
        }
    }
}
