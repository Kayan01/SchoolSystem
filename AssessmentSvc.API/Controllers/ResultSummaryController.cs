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
    public class ResultSummaryController : BaseController
    {
        private readonly IResultSummaryService _resultSummaryService;
        public ResultSummaryController(IResultSummaryService resultSummaryService)
        {
            _resultSummaryService = resultSummaryService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<List<StudentResultSummaryVM>>), 200)]
        public async Task<IActionResult> CalculateResultSummaries()
        {
            try
            {
                var result = await _resultSummaryService.CalculateResultSummaries();

                if (result.HasError)
                    return ApiResponse<List<StudentResultSummaryVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
