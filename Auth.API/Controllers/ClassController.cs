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

        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClassController(ISchoolClassService classService, IHttpContextAccessor httpContextAccesso)
        {
            _classService = classService;
            _httpContextAccessor = httpContextAccesso;
        }


        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> AddClass(ClassVM model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
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
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> AddClassGroup(ClassGroupVM model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _classService.AddClassGroup(model);
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
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> AddSection(ClassSectionVM model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _classService.AddSection(model);
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
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> AddStudentToClass(ClassStudentVM vm)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
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

        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> AssignSubjectToClass(ClassSubjectVM vm)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            }


            try
            {
                var result = await _classService.AssignSubjectToClass(vm);
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
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> AssignTeacherToClass(ClassTeacherVM vm)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            }


            try
            {
                var result = await _classService.AssignTeacherToClass(vm);
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
                var result = await _classService.DeleteClassGroup(id);
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
        public async Task<IActionResult> DeleteSection(long id)
        {
            if (id < 1)
            {
                return ApiResponse<string>(errors: "Please provide valid section Id");
            }


            try
            {
                var result = await _classService.DeleteSection(id);
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
        public async Task<IActionResult> GetAllClasses()
        {

            try
            {
                var result = await _classService.GetAllClasses();
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
                var result = await _classService.GetAllClassGroups();
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
                var result = await _classService.GetAllSections();
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
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> GetClassByIdWithStudents(long id)
        {
            if (id < 1)
            {
                return ApiResponse<string>(errors: "Please provide valid class Id");
            }

            try
            {
                var result = await _classService.GetAllClassGroups();
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
        public async Task<IActionResult> UpdateClass(ClassUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
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
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> UpdateClassGroup(ClassGroupVM model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _classService.UpdateClassGroup(model);
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
                var result = await _classService.UpdateSection(model);
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
