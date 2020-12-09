﻿using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.ViewModels.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentSvc.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class ResultController : BaseController
    {
        private readonly IResultService _resultService;
        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<ResultUploadFormData>), 200)]
        public async Task<IActionResult> GetResultUploadFormData([FromQuery]long classId)
        {
            try
            {
                var result = await _resultService.FetchResultUploadFormData(classId);
                if (result.HasError)
                    return ApiResponse<ResultUploadFormData>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: 1);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        public async Task<FileContentResult> GetResultUploadExcel([FromQuery] long classId, [FromQuery] string className)
        {
            try
            {
                var result = await _resultService.GenerateResultUploadExcel(classId);
                if (result.HasError)
                    return null;
                    //return ApiResponse<string>(errors: result.ErrorMessages.ToArray());


                string fileName = "ClassResultSheet.xlsx";
                if (!string.IsNullOrWhiteSpace(className))
                {
                    fileName = $"{className}ResultSheet.xlsx";
                }
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(result.Data, contentType, fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UploadAssessmentSetups([FromBody] ResultUploadVM model)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _resultService.ProcessResult(model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> PostResultFromExcel([FromBody] ResultFileUploadVM model)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _resultService.ProcessResultFromExcel(model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
