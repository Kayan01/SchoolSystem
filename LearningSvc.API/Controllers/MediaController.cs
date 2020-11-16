using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.Media;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace LearningSvc.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class MediaController : BaseController
    {
        private readonly IMediaService _mediaService;
        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> GetAllFileByTeacher([FromQuery] long teacherId, [FromQuery] QueryModel vM)
        {
            try
            {
                var result = await _mediaService.GetAllFileByTeacher(teacherId, vM);
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
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> GetAllFileByClass([FromQuery] long classId, [FromQuery] QueryModel vM)
        {
            try
            {
                var result = await _mediaService.GetAllFileByClass(classId, vM);
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
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> UploadFile([FromForm] MediaUploadVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _mediaService.UploadLearningFile(model, CurrentUser.UserId);

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
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> DeleteFile(long id)
        {
            if (id < 1)
            {
                return ApiResponse<string>(errors: "Please provide valid class Arm Id");
            }

            try
            {
                var result = await _mediaService.DeleteMedia(id);

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
