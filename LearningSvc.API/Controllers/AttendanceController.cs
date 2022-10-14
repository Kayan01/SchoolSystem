using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.Attendance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace LearningSvc.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class AttendanceController : BaseController
    {
        private readonly IAttendanceService _attendanceService;
        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UploadClassAttendance([FromBody] AddClassAttendanceVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _attendanceService.AddAttendanceForClass(model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<string>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UploadSubjectAttendance([FromBody] AddSubjectAttendanceVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _attendanceService.AddAttendanceForSubject(model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<string>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<GetStudentAttendanceSubjectVm>>), 200)]
        public async Task<IActionResult> GetStudentAttendanceForSubject([FromQuery] GetStudentAttendanceSubjectQueryVm model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _attendanceService.GetStudentAttendanceForSubject(model);

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
        [ProducesResponseType(typeof(ApiResponse<List<GetStudentAttendanceClassVm>>), 200)]
        public async Task<IActionResult> GetStudentAttendanceForClass([FromQuery] GetStudentAttendanceClassQueryVm model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _attendanceService.GetStudentAttendanceForClass(model);

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
        [ProducesResponseType(typeof(ApiResponse<List<StudentAttendanceSummaryVm>>), 200)]
        public async Task<IActionResult> GetClassAttendanceSummaryExcelReport([FromQuery] AttendanceRequestVM model)
        {
            try
            {
                var result = await _attendanceService.ExportStudentAttendanceReport(model);
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
        [ProducesResponseType(typeof(ApiResponse<List<StudentAttendanceSummaryVm>>), 200)]
        public async Task<IActionResult> GetClassAttendanceWithDateSummary([FromQuery] AttendanceRequestVM model)
        {
            try
            {
                var result = await _attendanceService.ExportClassAttendanceReport(model);
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
        [ProducesResponseType(typeof(ApiResponse<AttendanceExcelReport>), 200)]
        public async Task<IActionResult> AttendanceExport([FromQuery] AttendanceRequestVM model)
        {
            var payload = new AttendanceExcelReport();
            try
            {
                var res = await _attendanceService.ExportClassAttendanceReport(model); ;
                if (res.Data != null)
                {
                    var result = await _attendanceService.ExportAttendanceDataToExcel(res.Data);
                    if (result.HasError)
                        return ApiResponse<AttendanceExcelReport>(errors: result.ErrorMessages.ToArray());

                    payload = new AttendanceExcelReport
                    {
                        FileName = "AttendanceSummary",
                        Base64String = Convert.ToBase64String(result.Data)
                    };
                }
                return ApiResponse(data: payload, message: "Successful", totalCount: res.TotalCount);
            }
            catch (Exception ex)
            {

                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<StudentAttendanceSummaryVm>>), 200)]
        public async Task<IActionResult> GetClassSubjectAttendanceWithDateSummary([FromQuery] AttendanceRequestVM model)
        {
            try
            {
                var result = await _attendanceService.studentSubjectAttendanceView(model);
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
        [ProducesResponseType(typeof(ApiResponse<AttendanceExcelReport>), 200)]
        public async Task<IActionResult> SubjectAttendanceExport([FromQuery] AttendanceRequestVM model)
        {
            var payload = new AttendanceExcelReport();
            try
            {
                var res = await _attendanceService.studentSubjectAttendanceView(model); ;
                if (res.Data != null)
                {
                    var result = await _attendanceService.ExportAttendanceDataToExcel(res.Data);
                    if (result.HasError)
                        return ApiResponse<AttendanceExcelReport>(errors: result.ErrorMessages.ToArray());

                    payload = new AttendanceExcelReport
                    {
                        FileName = "AttendanceSummary",
                        Base64String = Convert.ToBase64String(result.Data)
                    };
                }
                return ApiResponse(data: payload, message: "Successful", totalCount: res.TotalCount);
            }
            catch (Exception ex)
            {

                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<AttendanceExcelReport>), 200)]
        public async Task<IActionResult> ClassAttendancePDFExport([FromQuery] AttendanceRequestVM model)
        {
            var payload = new AttendanceExcelReport();
            try
            {
                var res = await _attendanceService.ExportClassAttendanceReport(model);
                if (res.Data != null)
                {
                    var result = await _attendanceService.ExportAttendanceDataToPdf(res.Data);
                    if (result.HasError)
                        return ApiResponse<AttendanceExcelReport>(errors: result.ErrorMessages.ToArray());

                    payload = new AttendanceExcelReport
                    {
                        FileName = "AttendanceSummary",
                        Base64String = Convert.ToBase64String(result.Data)
                    };
                }
                return ApiResponse(data: payload, message: "Successful", totalCount: res.TotalCount);
            }
            catch (Exception ex)
            {

                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<AttendanceExcelReport>), 200)]
        public async Task<IActionResult> SubjectPDFAttendanceExport([FromQuery] AttendanceRequestVM model)
        {
            var payload = new AttendanceExcelReport();
            try
            {
                var res = await _attendanceService.studentSubjectAttendanceView(model);
                if (res.Data != null)
                {
                    var result = await _attendanceService.ExportAttendanceDataToPdf(res.Data);
                    if (result.HasError)
                        return ApiResponse<AttendanceExcelReport>(errors: result.ErrorMessages.ToArray());

                    payload = new AttendanceExcelReport
                    {
                        FileName = "AttendanceSummary",
                        Base64String = Convert.ToBase64String(result.Data)
                    };
                }
                return ApiResponse(data: payload, message: "Successful", totalCount: res.TotalCount);
            }
            catch (Exception ex)
            {

                return HandleError(ex);
            }
        }
    }
}
