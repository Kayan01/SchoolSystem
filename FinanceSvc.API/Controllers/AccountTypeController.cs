using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.AccountType;
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
    public class AccountTypeController : BaseController
    {
        private readonly IAccountTypeService _accountTypeService;
        public AccountTypeController(IAccountTypeService accountTypeService)
        {
            _accountTypeService = accountTypeService;
        }

        [HttpGet]
        [RequiresPermission(Permission.FINANCE_READ)]
        [ProducesResponseType(typeof(ApiResponse<List<AccountTypeVM>>), 200)]
        public async Task<IActionResult> GetAccountTypes()
        {
            try
            {
                var result = await _accountTypeService.GetAccountTypes();
                if (result.HasError)
                    return ApiResponse<List<AccountTypeVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{accountClassId}")]
        [RequiresPermission(Permission.FINANCE_READ)]
        [ProducesResponseType(typeof(ApiResponse<List<AccountTypeVM>>), 200)]
        public async Task<IActionResult> GetAccountTypes(long accountClassId)
        {
            try
            {
                var result = await _accountTypeService.GetAccountTypesByAccountClass(accountClassId);
                if (result.HasError)
                    return ApiResponse<List<AccountTypeVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        [RequiresPermission(Permission.FINANCE_READ)]
        [ProducesResponseType(typeof(ApiResponse<AccountTypeVM>), 200)]
        public async Task<IActionResult> GetAccountType(int id)
        {
            try
            {
                var result = await _accountTypeService.GetAccountType(id);
                if (result.HasError)
                    return ApiResponse<AccountTypeVM>(errors: result.ErrorMessages.ToArray());
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
        public async Task<IActionResult> NewAccountType([FromBody] AccountTypePostVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _accountTypeService.AddAccountType(model);

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
        public async Task<IActionResult> UpdateAccountType(long id, [FromBody] AccountTypePostVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _accountTypeService.UpdateAccountType(id, model);

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
