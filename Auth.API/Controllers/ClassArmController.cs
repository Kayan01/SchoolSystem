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

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassArmController : BaseController
    {
        private readonly IClassArmService _classArmService;

        public ClassArmController(IClassArmService classArmService)
        {
            _classArmService = classArmService;
        }

        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> AddClassGroup(ClassArmVM model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _classArmService.AddClassArm(model);

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
        public async Task<IActionResult> DeleteClassGroup(long id)
        {
            if (id < 1)
            {
                return ApiResponse<string>(errors: "Please provide valid class group Id");
            }

            try
            {
                var result = await _classArmService.DeleteClassArm(id);

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
        public async Task<IActionResult> GetAllClassGroups()
        {
            try
            {
                var result = await _classArmService.GetAllClassArms();

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
        public async Task<IActionResult> UpdateClassGroup(ClassArmVM model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _classArmService.UpdateClassGroup(model);

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