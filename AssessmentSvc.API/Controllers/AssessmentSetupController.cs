using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.ViewModels.AssessmentSetup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace AssessmentSvc.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [AllowAnonymous]
    public class AssessmentSetupController : BaseController
    {
        private readonly IAssessmentSetupService _assessmentSetupService;
        public AssessmentSetupController(IAssessmentSetupService assessmentSetupService)
        {
            _assessmentSetupService = assessmentSetupService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AssessmentSetupVM>>), 200)]
        public async Task<IActionResult> GetAllAssessmentSetup()
        {
            try
            {
                var result = await _assessmentSetupService.GetAllAssessmentSetup();
                if (result.HasError)
                    return ApiResponse<List<AssessmentSetupVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<AssessmentSetupVM>), 200)]
        public async Task<IActionResult> GetAssessmentSetup(long id)
        {
            try
            {
                var result = await _assessmentSetupService.GetAssessmentSetup(id);
                if (result.HasError)
                    return ApiResponse<AssessmentSetupVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<List<AssessmentSetupUploadVM>>), 200)]
        public async Task<IActionResult> UploadAssessmentSetups([FromBody] List<AssessmentSetupUploadVM> model)
        {
            if (model.Count <= 1)
                return ApiResponse<List<AssessmentSetupUploadVM>>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<List<AssessmentSetupUploadVM>>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _assessmentSetupService.AddAssessmentSetup(model);

                if (result.HasError)
                    return ApiResponse<List<AssessmentSetupUploadVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

    }
}
