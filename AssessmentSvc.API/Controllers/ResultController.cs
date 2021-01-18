using AssessmentSvc.Core.Interfaces;
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
        private readonly IApprovedResultService _approvedResultService;
        public ResultController(IResultService resultService, IApprovedResultService approvedResultService)
        {
            _resultService = resultService;
            _approvedResultService = approvedResultService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<ResultUploadFormData>), 200)]
        public async Task<IActionResult> GetResultUploadFormData([FromQuery] long classId)
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

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> SubmitStudentResult(UpdateApprovedStudentResultViewModel vm)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {

                var result = await _approvedResultService.SubmitStudentResult(vm);
                if (result.HasError)
                    return ApiResponse<ResultUploadFormData>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<GetApprovedStudentResultViewModel>), 200)]
        public async Task<IActionResult> GetStudentResultForApproval([FromQuery] GetStudentResultForApproval vm)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _approvedResultService.GetStudentResultForApproval(vm);
                if (result.HasError)
                    return ApiResponse<ResultUploadFormData>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpGet("{classId}")]
        [ProducesResponseType(typeof(ApiResponse<GetApprovedStudentResultViewModel>), 200)]
        public async Task<IActionResult> GetClassResultForApproval(long classId)
        {
            if (classId < 1)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _approvedResultService.GetClassTeacherApprovedClassBroadSheet(classId);
                if (result.HasError)
                    return ApiResponse<ResultUploadFormData>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<GetApprovedStudentResultViewModel>), 200)]
        public async Task<IActionResult> SubmitClassResultForApproval(UpdateApprovedClassResultViewModel vm)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _approvedResultService.SubmitClassResultForApproval(vm);
                if (result.HasError)
                    return ApiResponse<ResultUploadFormData>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetResultUploadExcel([FromQuery] long classId)
        {
            try
            {
                var result = await _resultService.GenerateResultUploadExcel(classId);
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: Convert.ToBase64String(result.Data), totalCount: 1);

            }
            catch (Exception ex)
            {
                return HandleError(ex);
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
        public async Task<IActionResult> PostResultFromExcel([FromForm] ResultFileUploadVM model)
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

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ApiResponse<List<ResultBroadSheet>>), 200)]
        public async Task<IActionResult> GetClassBroadSheet(long Id)
        {
            if (Id < 1)
                return ApiResponse<List<ResultBroadSheet>>(errors: "Please provide valid Id", codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _resultService.GetClassBroadSheet(Id);

                if (result.HasError)
                    return ApiResponse<List<ResultBroadSheet>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IndividualBroadSheet>), 200)]
        public async Task<IActionResult> GetStudentBroadSheet(long studId, long classId)
        {
            if (studId < 1 || classId < 1)
                return ApiResponse<IndividualBroadSheet>(errors: "Please provide valid Id", codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _resultService.GetStudentResultSheet(classId, studId);

                if (result.HasError)
                    return ApiResponse<IndividualBroadSheet>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
