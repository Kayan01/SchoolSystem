using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.Subject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace LearningSvc.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class SubjectController : BaseController
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SubjectVM>>), 200)]
        public async Task<IActionResult> GetSubjects([FromQuery] QueryModel vM)
        {
            try
            {
                var result = await _subjectService.GetAllSubjects(vM);
                if (result.HasError)
                    return ApiResponse<IEnumerable<SubjectVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<IEnumerable<SubjectVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<SubjectVM>>), 200)]
        public async Task<IActionResult> GetAllSubjects()
        {
            try
            {
                var result = await _subjectService.GetAllSubjects();
                if (result.HasError)
                    return ApiResponse<List<SubjectVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<SubjectVM>>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SubjectVM>), 200)]
        public async Task<IActionResult> NewSubject([FromBody] SubjectInsertVM model)
        {
            if (model == null)
                return ApiResponse<SubjectVM>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<SubjectVM>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _subjectService.AddSubject(model);

                if (result.HasError)
                    return ApiResponse<SubjectVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<SubjectVM>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

    }
}
