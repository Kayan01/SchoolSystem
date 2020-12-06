using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.ViewModels.SessionSetup;
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

    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class SessionController : BaseController
    {
        private readonly ISessionSetup _sessionRepo;
        public SessionController(ISessionSetup sessionRepo)
        {
            _sessionRepo = sessionRepo;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SessionSetupDetail>), 200)]
        public async Task<IActionResult> AddSchoolSession([FromBody] AddSessionSetupVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _sessionRepo.AddSchoolSession(model);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost("{Id}")]
        [ProducesResponseType(typeof(ApiResponse<SessionSetupDetail>), 200)]
        public async Task<IActionResult> UpdateSchoolSession(long Id,[FromBody] AddSessionSetupVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _sessionRepo.UpdateSchoolSession(Id, model);
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
        [ProducesResponseType(typeof(ApiResponse<List<SessionSetupDetail>>), 200)]
        public async Task<IActionResult> GetSchoolSessions()
        {

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _sessionRepo.GetSchoolSessionS();
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
        [HttpGet()]
        [ProducesResponseType(typeof(ApiResponse<List<SessionSetupDetail>>), 200)]
        public async Task<IActionResult> GetCurrentSchoolSessions(long Id)
        {

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _sessionRepo.GetCurrentSchoolSession();
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
