using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace AssessmentSvc.API.Controllers
{
    [Authorize]
    public class IncidenceController : BaseController
    {
        private readonly IIncidenceService _incidenceService;
        public IncidenceController(IIncidenceService incidenceService)
        {
            _incidenceService = incidenceService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<GetIncidenceVm>>), 200)]
        public async Task<IActionResult> GetIncidenceReports([FromQuery] GetIncidenceQueryVm model)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _incidenceService.GetIncidenceReport(model);

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
        [ProducesResponseType(typeof(ApiResponse<List<GetIncidenceVm>>), 200)]
        public async Task<IActionResult> InsertIncidenceReport([FromBody] AddIncidenceVm model)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _incidenceService.InsertIncidenceReport(model);

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
