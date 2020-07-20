using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.AspNetCore;
using Shared.FileStorage;
using Shared.Utils;
using Shared.ViewModels;
using Shared.ViewModels.Enums;
using FacilitySvc.API.ViewModel;
using NotificationSvc.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using NotificationSvc.Core.ViewModels;

namespace FacilitySvc.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class TestController : BaseController
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly INotificationService _notificationService;

        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger, IFileStorageService fileStorageService,
            INotificationService notificationService)
        {
            _logger = logger;
            _fileStorageService = fileStorageService;
            _notificationService = notificationService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> GetNotices()
        {
            try
            {
                var result = await _notificationService.GetNotifications();
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{title}")]
        [AllowAnonymous]
        public async Task<IActionResult> TestBroadcast(string title)
        {
            try
            {
                var result = await _notificationService.TestBroadcast(title);

                return ApiResponse<string>(message: "Successful", codes: ApiResponseCodes.OK, data: "Sent");
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> AddNotice(NoticeVM model)
        {
            try
            {
                var result = await _notificationService.AddNotification(model);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }



        [HttpPost()]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UploadFile([FromForm]CreateFileVM model)
        {
            var response = new ApiResponse<string>();
            if (ModelState.IsValid)
            {
                var userId = CurrentUser?.UserId;
                var filePaths = TryUploadSupportingDocuments(new List<IFormFile>() { model.Document });
                response.Payload = filePaths.FirstOrDefault();
                if (filePaths.Any())
                    return Ok(response);
            }
            return ApiResponse(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
        }


        private List<string> TryUploadSupportingDocuments(List<IFormFile> formFiles)
        {
            var filePaths = new List<string>();
            var uploaded = false;
            foreach (var file in formFiles)
            {
                var fileName = CommonHelper.GenerateTimeStampedFileName(file.FileName);
                uploaded = _fileStorageService.TrySaveStream(fileName,
                  file.OpenReadStream());
                if (uploaded)
                    filePaths.Add(fileName);
                else
                    break;
            }

            if (!uploaded && filePaths.Count() > 0)
            {
                foreach (var file in filePaths)
                    _fileStorageService.DeleteFile(file);
            }
            return filePaths;
        }
    }
}
