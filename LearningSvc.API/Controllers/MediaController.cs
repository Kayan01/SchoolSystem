﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.LearningFile;
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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MediaListVM>>), 200)]
        public async Task<IActionResult> GetAllFileByTeacher([FromQuery] QueryModel vM)
        {
            try
            {
                var result = await _mediaService.GetAllFileByTeacher(CurrentUser.UserId, vM);
                if (result.HasError)
                    return ApiResponse<IEnumerable<MediaListVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<IEnumerable<MediaListVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MediaListVM>>), 200)]
        public async Task<IActionResult> GetAllFileByClass([FromQuery] long classId, [FromQuery] QueryModel vM)
        {
            try
            {
                var result = await _mediaService.GetAllFileByClass(classId, vM);
                if (result.HasError)
                    return ApiResponse<IEnumerable<MediaListVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<IEnumerable<MediaListVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<StudentFileVM>>), 200)]
        public async Task<IActionResult> GetFileByClassSubject([FromQuery] long classSubjectId, [FromQuery] QueryModel vM)
        {
            try
            {
                var result = await _mediaService.GetAllFileByClassSubject(classSubjectId, vM);
                if (result.HasError)
                    return ApiResponse<List<StudentFileVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<MediaVM>), 200)]
        public async Task<IActionResult> GetMediaDetail([FromQuery] long id)
        {
            try
            {
                var result = await _mediaService.MediaDetail(id);
                if (result.HasError)
                    return ApiResponse<MediaVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UploadFile([FromForm] MediaUploadVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _mediaService.UploadLearningFile(model, CurrentUser.UserId);

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
                var result = await _mediaService.DeleteMedia(id);

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
