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
                promotionSetup = new PromotionSetup();
            }

            promotionSetup.PromotionMethod = vm.PromotionMethod;
            promotionSetup.PromotionType = vm.PromotionType;
            promotionSetup.PromotionScore = vm.PromotionScore;

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<PromotionSetupVM>((PromotionSetupVM)promotionSetup);
        }

        public async Task<ResultModel<WithdrawalSetupVM>> AddOrUpdateWithdrawalSetup(WithdrawalSetupVM vm)
        {
            var setup = await _promotionSetupRepo.GetAll().Include(m=>m.WithdrawalReasons).FirstOrDefaultAsync();

            if (setup == null)
            {
                setup = new PromotionSetup();
                setup.WithdrawalReasons = vm.WithdrawalReasons.Select(m => new WithdrawalReason() { Reason = m.reason }).ToList();
            }
            else
            {
                setup.WithdrawalReasons.RemoveAll(m => !vm.WithdrawalReasons.Any(n => n.id == m.Id));

                foreach (var reason in setup.WithdrawalReasons)
                {
                    //check if this reason still exists in the view model
                    var vmReason = vm.WithdrawalReasons.FirstOrDefault(m => m.id == reason.Id);

                    //if it does not exist, Remove it.
                    if (vmReason is null)
                    {
                        setup.WithdrawalReasons.Remove(reason);
                    }
                    else // update it.
                    {
                        reason.Reason = vmReason.reason;
                    }

                }

                // Add the reasons with id as 0
                var newReasons = vm.WithdrawalReasons.Where(m => m.id == 0);
                setup.WithdrawalReasons.AddRange(newReasons.Select(m => new WithdrawalReason() { Reason = m.reason }));
            }

            setup.MaxRepeat = vm.MaxRepeat;

            await _unitOfWork.SaveChangesAsync();

            setup = await _promotionSetupRepo.GetAll().Include(m => m.WithdrawalReasons).FirstOrDefaultAsync();

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
