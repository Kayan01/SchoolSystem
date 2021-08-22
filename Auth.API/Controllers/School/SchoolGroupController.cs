using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.SchoolGroup;
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
    public class SchoolGroupController : BaseController
    {
        private readonly ISchoolGroupService _schoolGroupService;

        public SchoolGroupController(ISchoolGroupService schoolGroupService)
        {
            _schoolGroupService = schoolGroupService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SchoolGroupListVM>), 200)]
        public async Task<IActionResult> AddSchoolGroup([FromForm]CreateSchoolGroupVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _schoolGroupService.AddSchoolGroup(model);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        //[HttpPost]
        //[ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        //public async Task<IActionResult> BulkAddSchoolGroup([FromForm]IFormFile file)
        //{
        //    if (file == null)
        //        return ApiResponse<string>(errors: "No file uploaded");

        //    if (!ModelState.IsValid)
        //        return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

        //    try
        //    {
        //        var result = await _schoolGroupService.AddBulkSchoolGroup(file);
        //        if (result.HasError)
        //            return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
        //        return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleError(ex);
        //    }
        //}

        [HttpGet]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SchoolGroupListVM>>), 200)]
        public async Task<IActionResult> GetSchoolGroups([FromQuery] QueryModel vM, [FromQuery] long? groupId)
        {
            try
            {
                var result = await _schoolGroupService.GetAllSchoolGroups(vM, groupId);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(string), 200)]
        //public async Task<IActionResult> GetSchoolGroupExcelUploadData()
        //{
        //    try
        //    {
        //        var result = await _schoolGroupService.GetSchoolGroupExcelSheet();
        //        if (result.HasError)
        //            return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
        //        return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: Convert.ToBase64String(result.Data), totalCount: 1);
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleError(ex);
        //    }
        //}

        //[HttpGet("{id}")]
        ////[Authorize]
        //[ProducesResponseType(typeof(ApiResponse<SchoolGroupDetailVM>), 200)]
        //public async Task<IActionResult> GetSchoolGroup(long id)
        //{
        //    if (id <= 0)
        //    {
        //        return ApiResponse<string>(errors: "Please provide valid schoolGroup Id");
        //    }

        //    try
        //    {
        //        var result = await _schoolGroupService.GetSchoolGroupById(id);
        //        if (result.HasError)
        //            return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
        //        return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleError(ex);
        //    }
        //}

        //[HttpGet("{id}")]
        //[ProducesResponseType(typeof(ApiResponse<SchoolGroupNameAndLogoVM>), 200)]
        //public async Task<IActionResult> GetSchoolGroupNameAndLogo(long id)
        //{
        //    if (id <= 0)
        //    {
        //        return ApiResponse<SchoolGroupNameAndLogoVM>(errors: "Please provide valid schoolGroup Id");
        //    }
        //    try
        //    {
        //        var result = await _schoolGroupService.GetSchoolGroupNameAndLogoById(id);
        //        if (result.HasError)
        //            return ApiResponse<SchoolGroupNameAndLogoVM>(errors: result.ErrorMessages.ToArray());
        //        return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleError(ex);
        //    }
        //}

        //[HttpGet("{domain}")]
        //[AllowAnonymous]
        //[ProducesResponseType(typeof(ApiResponse<SchoolGroupNameAndLogoVM>), 200)]
        //public async Task<IActionResult> GetSchoolGroupNameAndLogoByDomain(string domain)
        //{
        //    if (string.IsNullOrWhiteSpace(domain))
        //    {
        //        return ApiResponse<SchoolGroupNameAndLogoVM>(errors: "Please provide valid schoolGroup Domain");
        //    }
        //    try
        //    {
        //        var result = await _schoolGroupService.GetSchoolGroupNameAndLogoByDomain(domain);
        //        if (result.HasError)
        //            return ApiResponse<SchoolGroupNameAndLogoVM>(errors: result.ErrorMessages.ToArray());
        //        return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleError(ex);
        //    }
        //}

        [HttpPut("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<SchoolGroupListVM>), 200)]
        public async Task<IActionResult> UpdateSchoolGroup([FromForm]UpdateSchoolGroupVM vM,[FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            if (id < 1)
            {
                return ApiResponse<string>(errors: "Invalid schoolGroup Id");
            }

            try
            {
                var result = await _schoolGroupService.UpdateSchoolGroup(vM, id);
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