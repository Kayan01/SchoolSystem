using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSvc.Core.Enumerations;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.TimeTable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace LearningSvc.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class TimeTableController : BaseController
    {
        private readonly ITimeTableService _timeTableService;
        public TimeTableController(ITimeTableService tService)
        {
            _timeTableService = tService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> GetPeriods()
        {
            try
            {
                var result = await _timeTableService.GetAllPeriodForSchool();
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
        public async Task<IActionResult> UploadPeriod(List<PeriodVM> model)
        {
            if (model == null || model.Count < 0)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _timeTableService.SetupSchoolPeriods(model);

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
        public async Task<IActionResult> GetTimetableForTeacher(long teacherId)
        {
            try
            {
                var result = await _timeTableService.GetTimeTableCellsForTeacher(teacherId);
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
        public async Task<IActionResult> GetTimetableForClass(long classId)
        {
            try
            {
                var result = await _timeTableService.GetTimeTableCellsForClass(classId);
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
        public async Task<IActionResult> UploadNewTimetableForClass(List<TimeTableCellVM> model, [FromRoute]long classId)
        {
            if (model == null || model.Count < 0)
                return ApiResponse<string>(errors: "Empty payload");

            if (classId <= 0)
                return ApiResponse<string>(errors: "Class Id was not sent.");

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            if (classId <= 0 || model.Any(m=>m.SchoolClassId != classId) )
                return ApiResponse<string>(errors: "Class Id discrepancies.");

            try
            {
                var result = await _timeTableService.SetupTimeTableCellsByClass(model, classId);

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
        public async Task<IActionResult> GetAllClassesForTeacherByDay(long teacherId, WeekDays day)
        {
            try
            {
                var result = await _timeTableService.GetAllClassesForTeacherToday(teacherId, day);
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
        public async Task<IActionResult> GetNextClassesForTeacherByDay(long teacherId, WeekDays day, int currentPeriod, int count)
        {
            try
            {
                var result = await _timeTableService.GetNextClassesForTeacherToday(teacherId, day, currentPeriod, count);
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
