using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.Fee;
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
    public class FeeController : BaseController
    {
        private readonly IFeeService _feeService;
        public FeeController(IFeeService feeService)
        {
            _feeService = feeService;
        }

        [HttpGet]
        [RequiresPermission(Permission.FINANCE_READ)]
        [ProducesResponseType(typeof(ApiResponse<List<FeeVM>>), 200)]
        public async Task<IActionResult> GetFees()
        {
            try
            {
                var result = await _feeService.GetFees();
                if (result.HasError)
                    return ApiResponse<List<FeeVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpGet("{id}")]
        [RequiresPermission(Permission.FINANCE_READ)]
        [ProducesResponseType(typeof(ApiResponse<FeeWithComponentVM>), 200)]
        public async Task<IActionResult> GetFee(int id)
        {
            try
            {
                var result = await _feeService.GetFee(id);
                if (result.HasError)
                    return ApiResponse<FeeWithComponentVM>(errors: result.ErrorMessages.ToArray());
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
        public async Task<IActionResult> NewFee([FromBody] FeePostVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _feeService.AddFee(model);

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
        public async Task<IActionResult> UpdateFee(long id, [FromBody] FeePostVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _feeService.UpdateFee(id, model);

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
