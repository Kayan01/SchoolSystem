using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace UserManagement.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class StaffController : BaseController
    {
        private readonly IStaffService _staffService;
        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> AddStaff(StaffVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _staffService.AddStaff(model);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }



        [HttpGet]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> GetAllStaffInSchool([FromQuery] PagingVM vM)
        {

            try
            {
                var result = await _staffService.GetAllStaff(vM.PageNumber, vM.PageSize);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> GetStaffById(long staffId)
        {
            if (staffId < 1)
                return ApiResponse<string>(errors: "Please provide Staff Id");

            try
            {
                var result = await _staffService.GetStaffById(staffId);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> UpdateStaff(StaffUpdateVM vM)
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _staffService.UpdateStaff(vM);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpDelete("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> DeleteStaff( long id)
        {
            if (id == 0)
                return ApiResponse<string>(errors: "Invalid Id");

            try
            {
                var result = await _staffService.DeleteStaff(id);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

    }
}
