﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace UserManagement.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
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
        [ProducesResponseType(typeof(ApiResponse<StaffVM>), 200)]
        public async Task<IActionResult> AddStaff([FromForm]AddStaffVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StaffVM>>), 200)]
        public async Task<IActionResult> GetAllStaffInSchool([FromQuery] QueryModel vM)
        {

            try
            {
                var result = await _staffService.GetAllStaff(vM);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{staffId}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<StaffDetailVM>), 200)]
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

        [HttpGet("{userId}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<StaffNameAndSignatureVM>), 200)]
        public async Task<IActionResult> GetStaffNameAndSignatureByUserId(long userId)
        {
            if (userId < 1)
                return ApiResponse<string>(errors: "Please provide Staff's User Id");

            try
            {
                var result = await _staffService.GetStaffNameAndSignatureByUserId(userId);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<List<StaffNameAndSignatureVM>>), 200)]
        public async Task<IActionResult> GetStaffNamesAndSignaturesByUserIds([FromQuery]List<long> UserIds, [FromQuery]bool GetBytes = true)
        {
            try
            {
                var result = await _staffService.GetStaffNamesAndSignaturesByUserIds(UserIds, GetBytes);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{Id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<StaffVM>), 200)]
        public async Task<IActionResult> UpdateStaff([FromForm]StaffUpdateVM vM, [FromRoute] long Id)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _staffService.UpdateStaff(vM, Id);
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
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
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
        
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetStaffsExcelSheet()
        {
            try
            {
                var result = await _staffService.GetStaffExcelSheet();
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: Convert.ToBase64String(result.Data), totalCount: 1);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> BulkAddStaff([FromForm]IFormFile file)
        {
            if (file == null)
                return ApiResponse<string>(errors: "No file uploaded");

            if(!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _staffService.AddBulkStaff(file);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch(Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
