using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.AssignmentAnswer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace LearningSvc.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class AssignmentAnswerController : BaseController
    {
        private readonly IAssignmentAnswerService _assignmentAnswerService;
        private readonly IStudentService _studentService;
        public AssignmentAnswerController(IAssignmentAnswerService assignmentAnswerService, IStudentService studentService)
        {
            _assignmentAnswerService = assignmentAnswerService;
            _studentService = studentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<AssignmentAnswerListVM>>), 200)]
        public async Task<IActionResult> GetAllAssignmentAnswers(long assignmentId)
        {
            try
            {
                var result = await _assignmentAnswerService.GetAllAnswer(assignmentId);
                if (result.HasError)
                    return ApiResponse<List<AssignmentAnswerListVM>>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<List<AssignmentAnswerListVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<AssignmentAnswerVM>), 200)]
        public async Task<IActionResult> GetAssignmentAnswer(long answerId)
        {
            try
            {
                var result = await _assignmentAnswerService.GetAssignmentAnswer(answerId);
                if (result.HasError)
                    return ApiResponse<AssignmentAnswerVM>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<AssignmentAnswerVM>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UploadAssignmentAnswer([FromForm] AssignmentAnswerUploadVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _assignmentAnswerService.AddAssignmentAnswer(model, CurrentUser.UserId);

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
        public async Task<IActionResult> UpdateScore(AssignmentAnswerUpdateScoreVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _assignmentAnswerService.UpdateScore(model);

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
        public async Task<IActionResult> UpdateComment(AssignmentAnswerUpdateCommentVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _assignmentAnswerService.UpdateComment(model);

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
