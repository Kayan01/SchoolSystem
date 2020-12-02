using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.Assignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.Pagination;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace LearningSvc.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class AssignmentController : BaseController
    {
        private readonly IAssignmentService _assignmentService;
        private readonly IStudentService _studentService;
        public AssignmentController(IAssignmentService assignmentService, IStudentService studentService)
        {
            _assignmentService = assignmentService;
            _studentService = studentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<AssignmentGetVM>>), 200)]
        public async Task<IActionResult> GetAssignmentsByTeacher([FromQuery] QueryModel vM)
        {
            try
            {
                var result = await _assignmentService.GetAssignmentsForTeacher(CurrentUser.UserId, vM);
                if (result.HasError)
                    return ApiResponse<List<AssignmentGetVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<AssignmentGetVM>>), 200)]
        public async Task<IActionResult> GetAssignmentsByClass([FromQuery] long classId, [FromQuery] QueryModel vM)
        {
            try
            {
                if (classId < 1)
                {
                    classId = await _studentService.GetStudentClassIdByUserId(CurrentUser.UserId);
                }

                var result = await _assignmentService.GetAssignmentsForClass(classId, vM);
                if (result.HasError)
                    return ApiResponse<List<AssignmentGetVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<AssignmentGetVM>>), 200)]
        public async Task<IActionResult> GetAssignmentsByClassSubject([FromQuery] long classSubjectId, [FromQuery] QueryModel vM)
        {
            try
            {
                var result = await _assignmentService.GetAssignmentsForClassSubject(classSubjectId, vM);
                if (result.HasError)
                    return ApiResponse<List<AssignmentGetVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<AssignmentVM>>), 200)]
        public async Task<IActionResult> GetAssignmentDetail([FromQuery] long id)
        {
            try
            {
                var result = await _assignmentService.AssignmentDetail(id);
                if (result.HasError)
                    return ApiResponse<AssignmentVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UploadAssignment([FromForm] AssignmentUploadVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _assignmentService.AddAssignment(model, CurrentUser.UserId);

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
