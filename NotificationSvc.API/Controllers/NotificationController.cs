using Microsoft.AspNetCore.Mvc;
using NotificationSvc.Core.Services.Interfaces;
using NotificationSvc.Core.ViewModels;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationSvc.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<NotificationVM>>), 200)]
        public async Task<IActionResult> GetNotifications([FromQuery]PagedRequestModel model)
        {
            try
            {
                var result = await _notificationService.GetNotifications(model, CurrentUser.UserId);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<List<NotificationVM>>(message: result.Message, codes: ApiResponseCodes.OK, data: result.Data.Items.ToList(), totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<List<NotificationVM>>), 200)]
        public async Task<IActionResult> ReadNotification(long[] notificationIds)//FOR Testing notification
        {
            if (notificationIds == null)
                return ApiResponse<string>(errors: "NotificationIds is required");

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _notificationService.ReadNotification(notificationIds, CurrentUser.UserId);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<NotificationVM>>(message: result.Message, codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<List<NotificationVM>>), 200)]
        public async Task<IActionResult> CreateNotification(CreateNotificationModel model)//FOR Testing notification
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _notificationService.CreateNotification(model);

                if (result.HasError)
                    return ApiResponse<List<NotificationVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<List<NotificationVM>>(message: result.Message, codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}