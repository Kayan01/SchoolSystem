using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.ViewModels;
using AssessmentSvc.Core.ViewModels.Attendance;
using AssessmentSvc.Core.ViewModels.Result;
using AssessmentSvc.Core.ViewModels.Student;
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
        private readonly IAttendanceService _attendanceService;

        public ResultController(IResultService resultService, IApprovedResultService approvedResultService,IAttendanceService attendanceService)
        {
            _resultService = resultService;
            _approvedResultService = approvedResultService;
            _attendanceService = attendanceService;
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
        [ProducesResponseType(typeof(ApiResponse<List<ResultBroadSheet>>), 200)]
        public async Task<IActionResult> GetClassBroadSheetApprovedByClassTeacher(long classId)
        {
            if (classId < 1)
                return ApiResponse<List<ResultBroadSheet>>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _approvedResultService.GetClassTeacherApprovedClassBroadSheet(classId);
                if (result.HasError)
                    return ApiResponse<List<ResultBroadSheet>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> SubmitClassResultForApproval(UpdateApprovedClassResultViewModel vm)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: "Please enter valid id", codes: ApiResponseCodes.INVALID_REQUEST);

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

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<StudentReportSheetVM>), 200)]
        public async Task<IActionResult> GetApprovedStudentReportSheet(long? studId, long? studUserId, long classId, long? sessionId = null, int? termSequenceNumber = null)
        {
            if (studId < 1 || classId < 1)
                return ApiResponse<StudentReportSheetVM>(errors: "Please provide valid Id", codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _approvedResultService.GetApprovedResultForStudent(classId, studId, studUserId, sessionId, termSequenceNumber);

                if (result.HasError)
                    return ApiResponse<StudentReportSheetVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<StudentReportSheetVM>), 200)]
        public async Task<IActionResult> GetApprovedResultForMultipleStudents(long[] studId, long classId, long? sessionId = null, int? termSequenceNumber = null)
        {
           
            try
            {
                var result = await _approvedResultService.GetApprovedResultForMultipleStudents(classId, studId, sessionId, termSequenceNumber);

                if (result.HasError)
                    return ApiResponse<StudentReportSheetVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<StudentVM>>), 200)]
        public async Task<IActionResult> GetStudentsWithApprovedResult(long classId, long? sessionId = null, int? termSequenceNumber = null)
        {
            if (classId < 1)
                return ApiResponse<List<StudentVM>>(errors: "Please provide valid Id", codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _approvedResultService.GetStudentsWithApprovedResult(classId, sessionId, termSequenceNumber);

                if (result.HasError)
                    return ApiResponse<List<StudentVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> PostBehaviourResult([FromBody] AddBehaviourResultVM model)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _resultService.InsertBehaviouralResult(model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> GetBehaviourResult([FromQuery] GetBehaviourResultQueryVm model)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _resultService.GetBehaviouralResult(model);

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
        public async Task<IActionResult> PostMailResult([FromBody] MailResultVM model)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _approvedResultService.MailResult(model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> GetClassesResultApproval([FromQuery] long? curSessionId = null, [FromQuery]int? termSequenceNumber = null)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _approvedResultService.GetClassesResultApproval(curSessionId, termSequenceNumber);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<StudentAttendanceSummaryVm>>), 200)]
        public async Task<IActionResult> GetStudentAttendanceSummary(int studentId, int classId)
        {
            try
            {
                var result = await _attendanceService.GetStudentAttendanceSummary(studentId, classId);
                if (result.HasError)
                    return ApiResponse<List<StudentAttendanceSummaryVm>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {

                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<StudentAttendanceReportVM>>), 200)]
        public async Task<IActionResult> ExportStudentAttendanceReport([FromQuery] AttendanceRequestVM model)
        {
            try
            {
                var result = await _attendanceService.ExportStudentAttendanceReport(model);
                if (result.HasError)
                    return ApiResponse<List<StudentAttendanceReportVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {

                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<ExportViewModel>), 200)]
        public async Task<IActionResult> ExportBroadSheetExcel([FromQuery]long Id)
        {
            var result = new ResultModel<ExportViewModel>();
            try
            {
                var query = _approvedResultService.GetClassTeacherApprovedClassBroadSheet(Id).Result;
                if (query != null)
                {
                    result = await _approvedResultService.ExportBroadSheetExcel(query.Data);
                }
               
                if (result.HasError)
                    return ApiResponse<List<StudentAttendanceReportVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {

                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<ExportViewModel>), 200)]
        public async Task<IActionResult> ExportBroadSheetPDF([FromQuery] long Id)
        {
            var result = new ResultModel<ExportViewModel>();
            try
            {
                var query = _approvedResultService.GetClassTeacherApprovedClassBroadSheet(Id).Result;
                if (query != null)
                {
                    result = await _approvedResultService.ExportBroadSheetPDF(query.Data);
                }

                if (result.HasError)
                    return ApiResponse<List<StudentAttendanceReportVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {

                return HandleError(ex);
            }
        }
    }
}
