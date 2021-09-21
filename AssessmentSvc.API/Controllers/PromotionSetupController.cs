using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.ViewModels.PromotionSetup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentSvc.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class PromotionSetupController : BaseController
    {
        private readonly IPromotionSetupService _promotionSetupService;
        public PromotionSetupController(IPromotionSetupService promotionSetupService)
        {
            _promotionSetupService = promotionSetupService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PromotionSetupVM>), 200)]
        public async Task<IActionResult> GetPromotionSetup()
        {
            try
            {
                var result = await _promotionSetupService.GetPromotionSetup();
                if (result.HasError)
                    return ApiResponse<PromotionSetupVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<WithdrawalSetupVM>), 200)]
        public async Task<IActionResult> GetWithdrawalSetup()
        {
            try
            {
                var result = await _promotionSetupService.GetWithdrawalSetup();
                if (result.HasError)
                    return ApiResponse<WithdrawalSetupVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PromotionSetupVM>), 200)]
        public async Task<IActionResult> AddOrUpdatePromotionSetup([FromBody] PromotionSetupVM model)
        {
            if (model is null)
                return ApiResponse<PromotionSetupVM>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<PromotionSetupVM>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _promotionSetupService.AddOrUpdatePromotionSetup(model);

                if (result.HasError)
                    return ApiResponse<PromotionSetupVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<WithdrawalSetupVM>), 200)]
        public async Task<IActionResult> AddOrUpdateWithdrawalSetup([FromBody] WithdrawalSetupVM model)
        {
            if (model is null)
                return ApiResponse<WithdrawalSetupVM>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<WithdrawalSetupVM>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _promotionSetupService.AddOrUpdateWithdrawalSetup(model);

                if (result.HasError)
                    return ApiResponse<WithdrawalSetupVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
