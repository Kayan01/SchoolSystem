using System;
using System.Threading.Tasks;
using Auth.Core.Interfaces;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.Alumni;
using Auth.Core.ViewModels.AlumniEvent;
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
    public class AlumniEventController : BaseController
    {

        private readonly IAlumniEventService _alumniEventService;
        public AlumniEventController(IAlumniEventService alumniService)
        {
            _alumniEventService = alumniService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<AlumniEventDetailVM>), 200)]
        public async Task<IActionResult> AddEvent(AddEventVM vm)
        {

            if (vm == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            try
            {
                var result = await _alumniEventService.AddEvent(vm);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<AlumniEventDetailVM>), 200)]
        public async Task<IActionResult> GetAllEvents([FromQuery]QueryModel vm)
        {

            if (vm == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            try
            {
                var result = await _alumniEventService.GetAllEvents(vm);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AlumniEventDetailVM>), 200)]
        public async Task<IActionResult> GetEventById(long id)
        {
            if (id < 1)
                return ApiResponse<string>(errors: "Invalid student Id");

            try
            {
                var result = await _alumniEventService.GetEventsById(id);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AlumniEventDetailVM>), 200)]
        public async Task<IActionResult> UpdateEventById(long id, [FromBody] UpdateEventVM vm)
        {
            if (id < 1)
                return ApiResponse<string>(errors: "Invalid student Id");

            if (vm == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _alumniEventService.UpdateEventById(id, vm);

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
