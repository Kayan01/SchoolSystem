using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.Alumni;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.AspNetCore.Policy;
using Shared.Pagination;
using Shared.Permissions;
using Shared.ViewModels;
using Shared.ViewModels.Enums;


namespace Auth.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class AlumniController : BaseController
    {

        private readonly IAlumniService _alumniService;
        public AlumniController(IAlumniService alumniService)
        {
            _alumniService = alumniService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AlumniDetailVM>>), 200)]
        public async Task<IActionResult> GetAllAlumni([FromQuery] QueryModel model, [FromQuery] GetAlumniQueryVM queryVM)
        {

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            try
            {
                var result = await _alumniService.GetAllAlumni(model, queryVM);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.TotalCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpGet("{alumniId}")]
        [ProducesResponseType(typeof(ApiResponse<AlumniDetailVM>), 200)]
        public async Task<IActionResult> GetAllAlumniById(long alumniId)
        {

            if (alumniId < 1)
                return ApiResponse<string>(errors: "Please provide a alummi Id");

            try
            {
                var result = await _alumniService.GetAlumniById(alumniId);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<AlumniDetailVM>), 200)]
        public async Task<IActionResult> UpdateAlumni([FromForm] UpdateAlumniVM model)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _alumniService.UpdateAlumni(model);
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
        [ProducesResponseType(typeof(ApiResponse<AlumniDetailVM>), 200)]
        public async Task<IActionResult> AddAlumni([FromForm] AddAlumniVM model)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _alumniService.AddAlumni(model);
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
        [ProducesResponseType(typeof(ApiResponse<PastAlumniDetailVM>), 200)]
        public async Task<IActionResult> AddPastAlumni([FromForm] AddPastAlumniVM model, long schoolId)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _alumniService.AddPastStudents(model, schoolId);
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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AlumniDetailVM>>), 200)]
        public async Task<IActionResult> GetAllPastAlumni([FromQuery] QueryModel model, [FromQuery] GetAlumniQueryVM queryVM)
        {

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            try
            {
                var result = await _alumniService.GetAllPastAlumni(model, queryVM);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.TotalCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{pastAlumniId}")]
        [ProducesResponseType(typeof(ApiResponse<AlumniDetailVM>), 200)]
        public async Task<IActionResult> GetAllPastAlumniById(long pastAlumniId)
        {

            if (pastAlumniId < 1)
                return ApiResponse<string>(errors: "Please provide a alummi Id");

            try
            {
                var result = await _alumniService.GetPastAlumniById(pastAlumniId);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

    }
}
