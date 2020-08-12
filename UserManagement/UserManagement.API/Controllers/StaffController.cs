using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;
using UserManagement.Core.Services.Interfaces;
using UserManagement.Core.ViewModels.Staff;

namespace UserManagement.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class StaffController : BaseController
    {
        private readonly IStaffService _staffService;
        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> AddStaff(StaffVM model, long schoolId)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _staffService.AddStaff(schoolId, model);

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
        public async Task<IActionResult> GetAllStaff([FromQuery] long schId)
        {
            if (schId < 1)
                return ApiResponse<string>(errors: "Please provide school Id");

            try
            {
                var result = await _staffService.GetAllStaff(schId);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet()]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> GetStaffById([FromQuery] long schId, [FromQuery] long staffId)
        {
            if (schId < 1)
                return ApiResponse<string>(errors: "Please provide School Id");
            else if (staffId < 1)
                return ApiResponse<string>(errors: "Please provide Staff Id");

            try
            {
                var result = await _staffService.GetStaffById(staffId, schId);
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


        [HttpDelete]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> DeleteStaff([FromQuery] long schId, [FromQuery] long id)
        {
            if (id == 0)
                return ApiResponse<string>(errors: "Invalid Id");

            try
            {
                var result = await _staffService.DeleteStaff(schId,id);
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
