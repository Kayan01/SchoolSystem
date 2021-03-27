using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceSvc.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace FinanceSvc.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class FilesController : BaseController
    {
        private readonly IFileStore _fileStore;
        public FilesController(IFileStore fileStore)
        {
            _fileStore = fileStore;
        }
        [HttpGet("{Id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> GetFile(Guid Id)
        {
            if (Id == null)
            {
                return ApiResponse<object>(errors: "No Id provided", codes: ApiResponseCodes.INVALID_REQUEST);
            }


            try
            {
                var result = await _fileStore.GetFile(Id);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }

        }
    }
}
