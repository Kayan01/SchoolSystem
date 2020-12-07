using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.LessonNote;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace LearningSvc.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class LessonNoteController : BaseController
    {
        private readonly ILessonNoteService _lessonnoteService;
        public LessonNoteController(ILessonNoteService lessonnoteService)
        {
            _lessonnoteService = lessonnoteService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LessonNoteListVM>>), 200)]
        public async Task<IActionResult> GetAllFileByTeacher([FromQuery] QueryModel vM)
        {
            try
            {
                var result = await _lessonnoteService.GetAllFileByTeacher(CurrentUser.UserId, vM);
                if (result.HasError)
                    return ApiResponse<IEnumerable<LessonNoteListVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<IEnumerable<LessonNoteListVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LessonNoteListVM>>), 200)]
        public async Task<IActionResult> GetAllFileByClass([FromQuery] long classId,[FromQuery] QueryModel vM)
        {
            try
            {
                var result = await _lessonnoteService.GetAllFileByClass(classId, vM);
                if (result.HasError)
                    return ApiResponse<IEnumerable<LessonNoteListVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<IEnumerable<LessonNoteListVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<LessonNoteVM>), 200)]
        public async Task<IActionResult> GetLessonNoteDetail([FromQuery] long id)
        {
            try
            {
                var result = await _lessonnoteService.LessonNoteDetail(id);
                if (result.HasError)
                    return ApiResponse<LessonNoteVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UploadFile([FromForm] LessonNoteUploadVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _lessonnoteService.UploadLearningFile(model, CurrentUser.UserId);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<string>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpDelete("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> DeleteFile(long id)
        {
            if (id < 1)
            {
                return ApiResponse<string>(errors: "Please provide valid class Arm Id");
            }

            try
            {
                var result = await _lessonnoteService.DeleteLessonNote(id);

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
