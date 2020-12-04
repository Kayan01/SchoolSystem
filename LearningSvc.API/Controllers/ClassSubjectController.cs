using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.ClassSubject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace LearningSvc.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class ClassSubjectController : BaseController
    {
        private readonly IClassSubjectService _classSubjectService;
        private readonly IStudentService _studentService;
        public ClassSubjectController(IClassSubjectService classSubjectService, IStudentService studentService)
        {
            _classSubjectService = classSubjectService;
            _studentService = studentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ClassSubjectListVM>>), 200)]
        public async Task<IActionResult> GetAllClassSubjects()
        {
            try
            {
                var result = await _classSubjectService.GetAllClassSubjects();
                if (result.HasError)
                    return ApiResponse<List<ClassSubjectListVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<ClassSubjectListVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ClassSubjectListVM>>), 200)]
        public async Task<IActionResult> GetSubjectsForClass([FromQuery]long classId)
        {
            try
            {
                if (classId < 1)
                {
                    classId = await _studentService.GetStudentClassIdByUserId(CurrentUser.UserId);
                }

                var result = await _classSubjectService.GetSubjectsForClass(classId);
                if (result.HasError)
                    return ApiResponse<List<ClassSubjectListVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<ClassSubjectListVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ClassSubjectWithAssignmentCountVM>>), 200)]
        public async Task<IActionResult> GetSubjectsForClassWithAssignmentCount([FromQuery] long classId)
        {
            try
            {
                if (classId < 1)
                {
                    classId = await _studentService.GetStudentClassIdByUserId(CurrentUser.UserId);
                }

                var result = await _classSubjectService.GetSubjectsForClassWithAssignmentCount(classId);
                if (result.HasError)
                    return ApiResponse<List<ClassSubjectWithAssignmentCountVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<ClassSubjectWithAssignmentCountVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{subjectId}")]
        [ProducesResponseType(typeof(ApiResponse<List<ClassSubjectListVM>>), 200)]
        public async Task<IActionResult> GetClassesForSubject(long subjectId)
        {
            try
            {
                var result = await _classSubjectService.GetClassesForSubject(subjectId);
                if (result.HasError)
                    return ApiResponse<List<ClassSubjectListVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<ClassSubjectListVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> AddSubjectsToClass([FromBody] SubjectsToClassInsertVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _classSubjectService.AddSubjectsForClass(model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<string>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
