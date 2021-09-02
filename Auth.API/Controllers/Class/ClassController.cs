
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.SchoolClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace UserManagement.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [AllowAnonymous]
    [ApiController]
    public class ClassController : BaseController
    {
        private readonly ISchoolClassService _classService;

        public ClassController(ISchoolClassService classService)
        {
            _classService = classService;
        }

        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> AddClass([FromBody]AddClassVM model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
            var result = await _classService.AddClass(model);
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
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> AddStudentToClass([FromBody] ClassStudentVM vm)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _classService.AddStudentToClass(vm);
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
        ////[Authorize]
        //[ProducesResponseType(typeof(ApiResponse<object>), 200)]
        //public async Task<IActionResult> AssignTeacherToClass([FromForm] ClassTeacherVM vm)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
        //    }

        //    try
        //    {
        //        var result = await _classService.AssignTeacherToClass(vm);
        //        if (result.HasError)
        //            return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
        //        return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleError(ex);
        //    }
        //}

        [HttpDelete("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> DeleteClass(long id)
        {
            if (id < 1)
            {
                return ApiResponse<string>(errors: "Please provide valid class Id");
            }

            try
            {
                var result = await _classService.DeleteClass(id);
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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ListClassVM>>), 200)]
        public async Task<IActionResult> GetAllClasses([FromQuery]QueryModel vm)
        {
            try
            {
                var result = await _classService.GetAllClasses(vm);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ClassWithoutArmVM>>), 200)]
        public async Task<IActionResult> GetClassesWithoutArm()
        {
            try
            {
                var result = await _classService.GetClassesWithoutArm();
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<ListClassVM>>), 200)]
        public async Task<IActionResult> GetClassBySection(long id)
        {
            try
            {
                var result = await _classService.GetClassBySection(id);
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
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<ClassVM>), 200)]
        public async Task<IActionResult> GetClassById(long id)
        {
            if (id < 1)
            {
                return ApiResponse<string>(errors: "Please provide valid class Id");
            }

            try
            {
                var result = await _classService.GetClassById(id);
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
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<ListClassVM>), 200)]
        public async Task<IActionResult> GetClassByIdWithStudents(long id)
        {
            if (id < 1)
            {
                return ApiResponse<string>(errors: "Please provide valid class Id");
            }

            try
            {
                var result = await _classService.GetClassByIdWithStudents(id);
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
        [ProducesResponseType(typeof(ApiResponse<List<ClassUpdateVM>>), 200)]
        public async Task<IActionResult> UpdateClass([FromBody] ClassUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _classService.UpdateClass(model);
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
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UpdateClassSequenceAndTerminal([FromBody] List<ClassWithoutArmVM> model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _classService.UpdateClassSequenceAndTerminal(model);
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