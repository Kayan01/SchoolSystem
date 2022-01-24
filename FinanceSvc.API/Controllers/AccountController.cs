using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.Account;
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
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<AccountVM>>), 200)]
        [RequiresPermission(Permission.FINANCE_READ)]
        public async Task<IActionResult> GetAccounts()
        {
            try
            {
                var result = await _accountService.GetAccounts();
                if (result.HasError)
                    return ApiResponse<List<AccountVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{accountClassId}")]
        [ProducesResponseType(typeof(ApiResponse<List<AccountVM>>), 200)]
        [RequiresPermission(Permission.FINANCE_READ)]
        public async Task<IActionResult> GetAccounts(long accountClassId)
        {
            try
            {
                var result = await _accountService.GetAccountsByAccountClass(accountClassId);
                if (result.HasError)
                    return ApiResponse<List<AccountVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AccountVM>), 200)]
        [RequiresPermission(Permission.FINANCE_READ)]
        public async Task<IActionResult> GetAccount(int id)
        {
            try
            {
                var result = await _accountService.GetAccount(id);
                if (result.HasError)
                    return ApiResponse<AccountVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [RequiresPermission(Permission.FINANCE_CREATE)]
        public async Task<IActionResult> NewAccount([FromBody] AccountPostVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _accountService.AddAccount(model);

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
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [RequiresPermission(Permission.FINANCE_UPDATE)]
        public async Task<IActionResult> UpdateAccount(long id, [FromBody] AccountPostVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _accountService.UpdateAccount(id, model);

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
