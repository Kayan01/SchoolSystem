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
        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<AssignmentGetVM>>), 200)]
        public async Task<IActionResult> GetAssignmentsByTeacher([FromQuery] QueryModel vM)
        {
            try
            {
                var result = await _assignmentService.GetAssignmentsForTeacher(1, vM);
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
        [ProducesResponseType(typeof(ApiResponse<List<AssignmentSubmissionListVM>>), 200)]
        public async Task<IActionResult> GetAllAssignmentAnswers(long assignmentId)
        {
            try
            {
                var result = await _assignmentService.GetAllSubmission(assignmentId);
                if (result.HasError)
                    return ApiResponse<List<AssignmentSubmissionListVM>>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<List<AssignmentSubmissionListVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<AssignmentSubmissionVM>), 200)]
        public async Task<IActionResult> GetAssignmentAnswer(long answerId)
        {
            try
            {
                var result = await _assignmentService.GetAssignmentSubmission(answerId);
                if (result.HasError)
                    return ApiResponse<AssignmentSubmissionVM>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<AssignmentSubmissionVM>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
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

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UpdateScore(AssignmentSubmissionUpdateScoreVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _assignmentService.UpdateScore(model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<string>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UpdateComment(AssignmentSubmissionUpdateCommentVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _assignmentService.UpdateComment(model);

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
