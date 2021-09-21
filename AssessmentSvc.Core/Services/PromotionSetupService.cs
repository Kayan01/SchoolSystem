using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.ViewModels.PromotionSetup;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Services
{
    public class PromotionSetupService : IPromotionSetupService
    {
        private readonly IRepository<PromotionSetup, long> _promotionSetupRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PromotionSetupService(IUnitOfWork unitOfWork, IRepository<PromotionSetup, long> promotionSetupRepo)
        {
            _unitOfWork = unitOfWork;
            _promotionSetupRepo = promotionSetupRepo;
        }

        public async Task<ResultModel<PromotionSetupVM>> AddOrUpdatePromotionSetup(PromotionSetupVM vm)
        {
            var promotionSetup = await _promotionSetupRepo.GetAll().FirstOrDefaultAsync();

            if (promotionSetup == null)
            {
                promotionSetup = _promotionSetupRepo.Insert(new PromotionSetup());
            }

            promotionSetup.PromotionMethod = vm.PromotionMethod;
            promotionSetup.PromotionType = vm.PromotionType;
            promotionSetup.PromotionScore = vm.PromotionScore;

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<PromotionSetupVM>((PromotionSetupVM)promotionSetup);
        }

        public async Task<ResultModel<WithdrawalSetupVM>> AddOrUpdateWithdrawalSetup(WithdrawalSetupVM vm)
        {
            var setup = await _promotionSetupRepo.GetAll().FirstOrDefaultAsync();

            if (setup == null)
            {
                setup = _promotionSetupRepo.Insert(new PromotionSetup());
            }

            setup.MaxRepeat = vm.MaxRepeat;

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<WithdrawalSetupVM>((WithdrawalSetupVM)setup);
        }

        public async Task<ResultModel<PromotionSetupVM>> GetPromotionSetup()
        {
            var setup = await _promotionSetupRepo.GetAll().FirstOrDefaultAsync();
            return new ResultModel<PromotionSetupVM>((PromotionSetupVM)setup);
        }

        public async Task<ResultModel<WithdrawalSetupVM>> GetWithdrawalSetup()
        {
            var setup = await _promotionSetupRepo.GetAll().FirstOrDefaultAsync();
            return new ResultModel<WithdrawalSetupVM>((WithdrawalSetupVM)setup);
        }
    }
}
