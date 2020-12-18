using Auth.Core.Interfaces.Setup;
using Auth.Core.ViewModels.Setup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.API.Controllers.Setup
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SchoolPropertyController : BaseController
    {
        private readonly ISchoolPropertyService _schoolPropertyService;
        public SchoolPropertyController(ISchoolPropertyService schoolPropertyService)
        {
            _schoolPropertyService = schoolPropertyService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SchoolPropertyVM>), 200)]
        public async Task<IActionResult> SetSchoolProperty([FromBody] SchoolPropertyVM model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _schoolPropertyService.SetSchoolProperty(model);

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
        [ProducesResponseType(typeof(ApiResponse<SchoolPropertyVM>), 200)]
        public async Task<IActionResult> GetSchoolProperty()
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _schoolPropertyService.GetSchoolProperty();

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
