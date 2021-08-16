using System;
using System.Threading.Tasks;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.Alumni;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.AspNetCore.Policy;
using Shared.Pagination;
using Shared.Permissions;
using Shared.ViewModels;
using Shared.ViewModels.Enums;


namespace Auth.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class AlumniController : BaseController
    {

        private readonly IAlumniService _alumniService;
        public AlumniController(IAlumniService alumniService)
        {
            _alumniService = alumniService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<AlumniDetailVM>), 200)]
        public async Task<IActionResult> AddAlumni(AddAlumniVM vm)
        {

            if (vm == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
            try
            {
                var result = await _alumniService.AddAlumni(vm);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        

    }
}
