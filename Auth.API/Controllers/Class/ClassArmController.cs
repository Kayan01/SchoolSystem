using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Services.Interfaces.Class;
using Auth.Core.ViewModels.SchoolClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace Auth.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class ClassArmController : BaseController
    {
        private readonly IClassArmService _classArmService;

        public ClassArmController(IClassArmService classArmService)
        {
            _classArmService = classArmService;
        }

        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<ClassVM>), 200)]
        public async Task<IActionResult> AddClassArm([FromBody]AddClassArm model)
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

        [HttpDelete("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> DeleteClassArm(long id)
        {
            if (id < 1)
            {
                return ApiResponse<string>(errors: "Please provide valid class Arm Id");
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
        [ProducesResponseType(typeof(ApiResponse<List<ClassArmVM>>), 200)]
        public async Task<IActionResult> GetAllClassArms()
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

        [HttpGet("{Id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<ClassArmVM>), 200)]
        public async Task<IActionResult> GetClassArmById(long Id)
        {
            try
            {
                var result = await _classArmService.GetAllClassArmById(Id);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{Id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<ClassArmVM>), 200)]
        public async Task<IActionResult> UpdateClassArm([FromBody]UpdateClassArmVM model, long Id)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _classArmService.UpdateClassArm(model, Id);

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