using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.ViewModels.GradeSetup;
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
    public class GradeSetupController : BaseController
    {
        private readonly IGradeSetupService _gradeSetupService;
        public GradeSetupController(IGradeSetupService gradeSetupService)
        {
            _gradeSetupService = gradeSetupService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<List<GradeSetupListVM>>), 200)]
        public async Task<IActionResult> AddGradeSetup([FromBody]List<GradeSetupVM> model)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST); 
            try
            {
                var result = await _gradeSetupService.AddGradeSetup(model);
                if (result.HasError)
                    return ApiResponse<List<GradeSetupVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<GradeSetupListVM>>), 200)]
        public async Task<IActionResult> GetAllGradeForSchoolSetup()
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            try
            {
                var result = await _gradeSetupService.GetAllGradeForSchoolSetup();
                if (result.HasError)
                    return ApiResponse<List<GradeSetupListVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }



        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ApiResponse<GradeSetupVM>), 200)]
        public async Task<IActionResult> GetGradeSetupById([FromRoute] long Id)
        {
            try
            {
                var result = await _gradeSetupService.GetGradeSetupById(Id);
                if (result.HasError)
                    return ApiResponse<GradeSetupVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        //[HttpPost]
        //[ProducesResponseType(typeof(ApiResponse<List<GradeSetupListVM>>), 200)]
        //public async Task<IActionResult> AddGradeSetup([FromBody] List<GradeSetupVM> model)
        //{
        //    if (!ModelState.IsValid)
        //        return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
        //    try
        //    {
        //        var result = await _gradeSetupService.UpdateGradeSetup(model);
        //        if (result.HasError)
        //            return ApiResponse<List<GradeSetupVM>>(errors: result.ErrorMessages.ToArray());
        //        return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleError(ex);
        //    }
        //}



    }
}
