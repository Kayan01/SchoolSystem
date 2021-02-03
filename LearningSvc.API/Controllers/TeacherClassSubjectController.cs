using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.TeacherClassSubject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace LearningSvc.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class TeacherClassSubjectController : BaseController
    {
        private readonly ITeacherClassSubjectService _teacherClassSubjectService;
        private readonly ITeacherService _teacherService;
        public TeacherClassSubjectController(ITeacherClassSubjectService teacherClassSubjectService, ITeacherService teacherService)
        {
            _teacherClassSubjectService = teacherClassSubjectService;
            _teacherService = teacherService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<TeacherClassSubjectListVM>>), 200)]
        public async Task<IActionResult> GetAllClassSubjectsForTeacher([FromQuery]long teacherid)
        {
            try
            {
                if (teacherid < 1)
                {
                    teacherid = await _teacherService.GetTeacherIdByUserId(CurrentUser.UserId);
                }
                if (teacherid < 1)
                {
                    return ApiResponse<List<TeacherClassSubjectListVM>>(errors: new[] { "Logged in user is not a teacher." });
                }

                var result = await _teacherClassSubjectService.GetAllTeacherClassSubjects(teacherid);
                if (result.HasError)
                    return ApiResponse<List<TeacherClassSubjectListVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<TeacherClassSubjectListVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{classSubjectId}")]
        [ProducesResponseType(typeof(ApiResponse<List<TeacherClassSubjectListVM>>), 200)]
        public async Task<IActionResult> GetTeachersForClassSubject(long classSubjectId)
        {
            try
            {
                var result = await _teacherClassSubjectService.GetTeachersForClassSubject(classSubjectId);
                if (result.HasError)
                    return ApiResponse<List<TeacherClassSubjectListVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> AddClassSubjectsToTeacher([FromBody] TeacherClassSubjectInsertVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _teacherClassSubjectService.AddTeacherToClassSubjects(model);

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
