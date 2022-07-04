using Auth.Core.Interfaces.SchoolReport;
using Auth.Core.ViewModels.ReportDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.API.Controllers.School
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class SchoolReportController : BaseController
    {
        private readonly IReportService _reportService;

        public SchoolReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<SchoolReportVM>), 200)]
        public async Task<IActionResult> GetSchoolReportById([FromQuery]long SchoolId)
        {
            try
            {
               var resultModel = await _reportService.generateSchoolReport(SchoolId);
                if (resultModel.HasError)
                    return ApiResponse<object>(errors: resultModel.ErrorMessages.ToArray());

                return ApiResponse<object>(message: "Suceessful", codes: ApiResponseCodes.OK, data: resultModel.Data, totalCount: resultModel.TotalCount);  
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            } 
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<SchoolReportVM>), 200)]
        public async Task<IActionResult> GetClassReportById([FromQuery] long ClassId)
        {
            try
            {
                var resultModel = await _reportService.getClassReport(ClassId);
                if (resultModel.HasError)
                    return ApiResponse<object>(errors: resultModel.ErrorMessages.ToArray());

                return ApiResponse<object>(message: "Suceessful", codes: ApiResponseCodes.OK, data: resultModel.Data, totalCount: resultModel.TotalCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AdminLevelSchoolsReport>>), 200)]
        public async Task<IActionResult> GetSchoolDetailsForAdmin()
        {
            try
            {
                var resultModel = await _reportService.getSchoolDetailsForAdmin();
                if (resultModel.HasError)
                    return ApiResponse<object>(errors: resultModel.ErrorMessages.ToArray());

                return ApiResponse<object>(message: "Suceessful", codes: ApiResponseCodes.OK, data: resultModel.Data, totalCount: resultModel.Data.Count());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

    }
}
