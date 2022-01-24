using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.FeeGroup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.AspNetCore.Policy;
using Shared.Permissions;
using Shared.ViewModels;
using Shared.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceSvc.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class FeeGroupController : BaseController
    {
        private readonly IFeeGroupService _feeGroupService;
        public FeeGroupController(IFeeGroupService feeGroupService)
        {
            _feeGroupService = feeGroupService;
        }

        [HttpGet]
        [RequiresPermission(Permission.FINANCE_READ)]
        [ProducesResponseType(typeof(ApiResponse<List<FeeGroupVM>>), 200)]
        public async Task<IActionResult> GetFeeGroups()
        {
            try
            {
                var result = await _feeGroupService.GetFeeGroups();
                if (result.HasError)
                    return ApiResponse<List<FeeGroupVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        [RequiresPermission(Permission.FINANCE_READ)]
        [ProducesResponseType(typeof(ApiResponse<FeeGroupVM>), 200)]
        public async Task<IActionResult> GetFeeGroup(int id)
        {
            try
            {
                var result = await _feeGroupService.GetFeeGroup(id);
                if (result.HasError)
                    return ApiResponse<FeeGroupVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [RequiresPermission(Permission.FINANCE_CREATE)]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> NewFeeGroup([FromBody] FeeGroupPostVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _feeGroupService.AddFeeGroup(model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<string>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}")]
        [RequiresPermission(Permission.FINANCE_UPDATE)]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UpdateFeeGroup(long id, [FromBody] FeeGroupPostVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _feeGroupService.UpdateFeeGroup(id, model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<string>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


    }
}
