using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Interfaces.Setup;
using Auth.Core.ViewModels.Setup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace Auth.API.Controllers.Setup
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class DepartmentController : BaseController
    {
        public IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<DepartmentVM>), 200)]
        public async Task<IActionResult> AddDepartment([FromBody]AddDepartmentVM model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _departmentService.AddDepartment(model);

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
        public async Task<IActionResult> DeleteDepartment(long id)
        {
            if (id < 1)
            {
                return ApiResponse<string>(errors: "Please provide valid department Id");
            }

            try
            {
                var result = await _departmentService.DeleteDepartment(id);

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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DepartmentListVM>>), 200)]
        public async Task<IActionResult> GetAllDepartments([FromQuery]QueryModel queryModel)
        {
            try
            {
                var result = await _departmentService.GetAllDepartments(queryModel);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<DepartmentVM>), 200)]
        public async Task<IActionResult> GetDepartmentById(long Id)
        {
            try
            {
                var result = await _departmentService.GetDepartmentById(Id);

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
        [ProducesResponseType(typeof(ApiResponse<DepartmentVM>), 200)]
        public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentVM model, [FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _departmentService.UpdateDepartment(model, id);

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
