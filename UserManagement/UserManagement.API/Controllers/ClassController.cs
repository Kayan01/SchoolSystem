using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;
using UserManagement.Core.Services.Interfaces;

namespace UserManagement.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class ClassController : BaseController
    {
        private readonly ISchoolClassService _classService;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClassController(ISchoolClassService classService, IHttpContextAccessor httpContextAccesso)
        {
            _classService = classService;
            _httpContextAccessor = httpContextAccesso;
        }


        [HttpGet]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> GetAllClasses(long? schoolId)
        {


            if (schoolId < 1 || schoolId == null)
                return ApiResponse<string>(errors: "Please provide school Id");


            try
            {
                var result = await _classService.GetAllClasses(schoolId.Value);
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
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> GetTenants()
        {
            try
            {
                var result = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("tenantId", out StringValues tenantId);

                if (result == true)
                {
                    var tId = tenantId.First();
                    return ApiResponse<string>(tId);
                }

                return ApiResponse<string>(errors: "No Tenant Id");


            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

    }
}
