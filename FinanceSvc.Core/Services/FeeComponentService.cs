using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
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
    public class FeeComponentService: IFeeComponentService
    {
        private readonly IRepository<FeeComponent, long> _feeComponentRepo;
        private readonly IUnitOfWork _unitOfWork;

        public FeeComponentService(IUnitOfWork unitOfWork, IRepository<FeeComponent, long> feeComponentRepo)
        {
            _unitOfWork = unitOfWork;
            _feeComponentRepo = feeComponentRepo;
        }

        public async Task<ResultModel<string>> UpdateFeeComponent(long Id, FeeComponentPostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _feeComponentRepo.GetAll().Where(m => m.Id == Id).FirstOrDefaultAsync();

            if (check == null)
            {
                result.AddError("FeeComponent not found!");
                return result;
            }

            check.IsCompulsory = model.IsCompulsory;
            check.Amount = model.Amount;
            check.ComponentId = model.ComponentId;

            _feeComponentRepo.Update(check);

            await _unitOfWork.SaveChangesAsync();

            result.Data = "Update successful";
            return result;
        }
    }
}
