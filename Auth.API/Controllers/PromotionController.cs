using Auth.Core.Interfaces;
using Auth.Core.ViewModels.Promotion;
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

namespace Auth.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PromotionController : BaseController
    {
        private readonly IPromotionService _promotionService;
        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet("{sessionId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ClassPoolVM>>), 200)]
        public async Task<IActionResult> GetClassPool(long sessionId, [FromQuery] QueryModel vM, [FromQuery] long? classId)
        {
            try
            {
                var result = await _promotionService.GetClassPool(vM, sessionId, classId);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> PostClassPool([FromBody] List<ClassPoolVM> model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _promotionService.PostClassPool(model);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{sessionId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ClassPoolVM>>), 200)]
        public async Task<IActionResult> GetRepeatList(long sessionId, [FromQuery] QueryModel vM, [FromQuery] long? classId)
        {
            try
            {
                var result = await _promotionService.GetRepeatList(vM, sessionId, classId);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{sessionId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ClassPoolVM>>), 200)]
        public async Task<IActionResult> GetWithdrawnList(long sessionId, [FromQuery] QueryModel vM, [FromQuery] long? classId)
        {
            try
            {
                var result = await _promotionService.GetWithdrawnList(vM, sessionId, classId);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{sessionId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PromotionHighlightVM>>), 200)]
        public async Task<IActionResult> GetPromotionHighlight(long sessionId, [FromQuery] long? classId)
        {
            try
            {
                var result = await _promotionService.GetPromotionHighlight(sessionId, classId);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

    }
}
