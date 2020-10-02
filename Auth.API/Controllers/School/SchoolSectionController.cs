using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Services.Interfaces.Class;
using Auth.Core.ViewModels.SchoolClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace Auth.API.Controllers.School
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class SchoolSectionController : BaseController
    {
        private readonly ISectionService _sectionService;

        public SchoolSectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> AddSection(ClassSectionVM model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _sectionService.AddSection(model);

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
        public async Task<IActionResult> DeleteSection(long id)
        {
            if (id < 1)
            {
                return ApiResponse<string>(errors: "Please provide valid section Id");
            }

            try
            {
                var result = await _sectionService.DeleteSection(id);

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
        public async Task<IActionResult> GetAllSections()
        {
            try
            {
                var result = await _sectionService.GetAllSections();

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
        public async Task<IActionResult> UpdateSection(ClassSectionUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _sectionService.UpdateSection(model);

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