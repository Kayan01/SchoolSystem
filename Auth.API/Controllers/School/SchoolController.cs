using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.School;
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
    public class SchoolController : BaseController
    {
        private readonly ISchoolService _schoolService;

        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SchoolVM>), 200)]
        public async Task<IActionResult> AddSchool([FromForm]CreateSchoolVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _schoolService.AddSchool(model);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> BulkAddSchool([FromForm]IFormFile file)
        {
            if (file == null)
                return ApiResponse<string>(errors: "No file uploaded");

            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _schoolService.AddBulkSchool(file);
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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SchoolVM>>), 200)]
        public async Task<IActionResult> GetSchools([FromQuery] QueryModel vM)
        {
            try
            {
                var result = await _schoolService.GetAllSchools(vM);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<SchoolDetailVM>), 200)]
        public async Task<IActionResult> GetSchool(long id)
        {
            if (id <= 0)
            {
                return ApiResponse<string>(errors: "Please provide valid school Id");
            }

            try
            {
                var result = await _schoolService.GetSchoolById(id);
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
        [ProducesResponseType(typeof(ApiResponse<SchoolNameAndLogoVM>), 200)]
        public async Task<IActionResult> GetSchoolNameAndLogo(long id)
        {
            if (id <= 0)
            {
                return ApiResponse<SchoolNameAndLogoVM>(errors: "Please provide valid school Id");
            }
            try
            {
                var result = await _schoolService.GetSchoolNameAndLogoById(id);
                if (result.HasError)
                    return ApiResponse<SchoolNameAndLogoVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<SchoolVM>), 200)]
        public async Task<IActionResult> UpdateSchool([FromForm]UpdateSchoolVM vM,[FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            if (id < 1)
            {
                return ApiResponse<string>(errors: "Invalid school Id");
            }

            try
            {
                var result = await _schoolService.UpdateSchool(vM, id);
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
        public async Task<IActionResult> DeleteSchool(long id)
        {
            if (id < 1)
                return ApiResponse<string>(errors: "Invalid school Id");

            try
            {
                var result = await _schoolService.DeleteSchool(id);
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