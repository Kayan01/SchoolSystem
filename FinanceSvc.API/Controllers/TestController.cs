using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels.Enums;
using Microsoft.AspNetCore.Authorization;

namespace FinanceSvc.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class TestController : BaseController
    {

        public TestController() { }

        [HttpGet("{title}")]
        [AllowAnonymous]
        public async Task<IActionResult> TestBroadcast(string title)
        {
            try
            {
                return ApiResponse<string>(message: "Successful", codes: ApiResponseCodes.OK, data: "Sent");
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        
    }
}
