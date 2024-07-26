using FinanceSvc.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;
using System;
using System.Threading.Tasks;

namespace FinanceSvc.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class BroadCastDataController : BaseController
    {
        private readonly IBroadcastDateService _broadcastDateService;
        public BroadCastDataController(IBroadcastDateService broadcastDateService) 
        {
            _broadcastDateService = broadcastDateService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> GetDataNotBroadcastedFromAuth([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var result = await _broadcastDateService.GetDataFromAuth(startDate, endDate);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.TotalCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
