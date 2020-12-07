using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSvc.Core.Enumerations;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.TimeTable;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(typeof(ApiResponse<List<PeriodVM>>), 200)]
        public async Task<IActionResult> GetPeriods()
        {
            try
            {
                var result = await _timeTableService.GetAllPeriodForSchool();
                if (result.HasError)
                    return ApiResponse<List<PeriodVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<PeriodVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<List<PeriodVM>>), 200)]
        public async Task<IActionResult> UploadPeriod([FromBody] List<PeriodInsertVM> model)
        {
            if (model == null || model.Count < 0)
                return ApiResponse<List<PeriodVM>>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<List<PeriodVM>>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _timeTableService.SetupSchoolPeriods(model);

                if (result.HasError)
                    return ApiResponse<List<PeriodVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<PeriodVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{teacherId}")]
        [ProducesResponseType(typeof(ApiResponse<List<TimeTableCellVM>>), 200)]
        public async Task<IActionResult> GetTimetableForTeacher()
        {
            try
            {
                var result = await _timeTableService.GetTimeTableCellsForTeacher(CurrentUser.UserId);
                if (result.HasError)
                    return ApiResponse<List<TimeTableCellVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<TimeTableCellVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{classId}")]
        [ProducesResponseType(typeof(ApiResponse<List<TimeTableCellVM>>), 200)]
        public async Task<IActionResult> GetTimetableForClass(long classId)
        {
            try
            {
                var result = await _timeTableService.GetTimeTableCellsForClass(classId);
                if (result.HasError)
                    return ApiResponse<List<TimeTableCellVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<TimeTableCellVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<TimeTableCellVM>), 200)]
        public async Task<IActionResult> AddNewTimetableCell([FromBody] TimeTableCellInsertVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<TimeTableCellVM>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _timeTableService.AddTimeTableCell(model);

                if (result.HasError)
                    return ApiResponse<TimeTableCellVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<TimeTableCellVM>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ClassSessionOutputVM>>), 200)]
        public async Task<IActionResult> GetAllClassesForTeacherByDay(WeekDays day)
        {
            try
            {
                var result = await _timeTableService.GetAllClassesForTeacherToday(CurrentUser.UserId, day);
                if (result.HasError)
                    return ApiResponse<List<ClassSessionOutputVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<ClassSessionOutputVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ClassSessionOutputVM>>), 200)]
        public async Task<IActionResult> GetNextClassesForTeacherByDay(WeekDays day, int currentPeriod, int count)
        {
            try
            {
                var result = await _timeTableService.GetNextClassesForTeacherToday(CurrentUser.UserId, day, currentPeriod, count);
                if (result.HasError)
                    return ApiResponse<List<ClassSessionOutputVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<ClassSessionOutputVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ClassSessionOutputVM>>), 200)]
        public async Task<IActionResult> GetAllClassesForClassToday(long classId, WeekDays day)
        {
            try
            {
                var result = await _timeTableService.GetAllClassesForClassToday(classId, day);
                if (result.HasError)
                    return ApiResponse<List<ClassSessionOutputVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<ClassSessionOutputVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ClassSessionOutputVM>>), 200)]
        public async Task<IActionResult> GetNextClassesForClassToday(long classId, WeekDays day, int currentPeriod, int count)
        {
            try
            {
                var result = await _timeTableService.GetNextClassesForClassToday(classId, day, currentPeriod, count);
                if (result.HasError)
                    return ApiResponse<List<ClassSessionOutputVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<ClassSessionOutputVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpDelete("{timeTableCellId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> DeleteTimetableCell(long timeTableCellId)
        {
            try
            {
                var result = await _timeTableService.DeleteTimeTableCell(timeTableCellId);
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
