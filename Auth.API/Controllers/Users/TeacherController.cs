﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Interfaces.Users;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace Auth.API.Controllers.Users
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class TeacherController : BaseController
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TeacherVM>>), 200)]
        public async Task<IActionResult> GetTeachers([FromQuery]QueryModel model)
        {

            try
            {
                var result = await _teacherService.GetTeachers(model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<IEnumerable<TeacherVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount:result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ApiResponse<TeacherVM>), 200)]
        public async Task<IActionResult> GetTeachers(long Id)
        {

            try
            {
                var result = await _teacherService.GetTeacherById(Id);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<TeacherDetailVM>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ApiResponse<ClassTeacherVM>), 200)]
        public async Task<IActionResult> GetTeacherClass(long Id)
        {
            try
            {
                var result = await _teacherService.GetTeacherClassById(Id);

                if (result.HasError)
                    return ApiResponse<ClassTeacherVM>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<ClassTeacherVM>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<TeacherVM>), 200)]
        public async Task<IActionResult> AddTeacher([FromForm]AddStaffVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _teacherService.AddTeacher(model);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TeacherVM>), 200)]
        public async Task<IActionResult> UpdateTeacher([FromForm]UpdateTeacherVM model, [FromRoute] long id)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _teacherService.UpdateTeacher(model, id);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<TeacherVM>), 200)]
        public async Task<IActionResult> SetClassTeacher([FromBody] ClassTeacherVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _teacherService.MakeClassTeacher(model);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> DeleteTeacher(long userId)
        {
            if(userId < 1)
                return ApiResponse<string>(errors: "Invalid UserId");

            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _teacherService.DeleteTeacher(userId);

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
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetTeachersExcelSheet()
        {
            try
            {
                var result = await _teacherService.GetTeachersExcelSheet();
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
        [ProducesResponseType(typeof(ApiResponse<bool>),200)]
        public async Task<IActionResult> BulkAddTeacher([FromForm] IFormFile file)
        {
            if (file == null)
                return ApiResponse<string>(errors: "No file Uploaded");

            if (!ModelState.IsValid)
                return ApiResponse<Object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _teacherService.AddBulkTeacher(file);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch(Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<ExportPayloadVM>), 200)]
        public async Task<IActionResult> GetTeachersDataInExcel([FromQuery]StaffTypeVM model)
        {
            var result = new ResultModel<ExportPayloadVM>();
            try
            {
                var teacherData = await _teacherService.GetAllTeacherData(model);
                if (result != null)
                {
                    result = await _teacherService.ExportTeacherDataExcel(teacherData.Data);
                }

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.TotalCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<ExportPayloadVM>), 200)]
        public async Task<IActionResult> GetTeachersDataInPDF([FromQuery] StaffTypeVM model)
        {
            var result = new ResultModel<ExportPayloadVM>();
            try
            {
                var teacherData = await _teacherService.GetAllTeacherData(model);
                if (result != null)
                {
                    result = await _teacherService.ExportTeacherDataPDF(teacherData.Data);
                }

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.TotalCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(ApiResponse<TeacherVMDetails>), 200)]
        public async Task<IActionResult> GetTeacherDetailsByUserId(long userId)
        {
            try
            {
                var result = await _teacherService.GetTeacherIdByUserId(userId);

                if (result.HasError)
                    return ApiResponse<TeacherVMDetails>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<TeacherVMDetails>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

    }
}