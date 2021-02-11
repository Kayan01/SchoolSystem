using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FacilitySvc.Core.Services.Interfaces;
using FacilitySvc.Core.ViewModels;
using Shared.PubSub;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacilitySvc.Core.EventHandlers
{
    public class NoticeHandler
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NoticeHandler> _logger;

        public NoticeHandler(INotificationService noticeService, ILogger<NoticeHandler> logger)
        {
            _notificationService = noticeService;
            _logger = logger;
        }

        public void HandleTest(BusMessage message)
        {
            try
            {
                var notice = JsonConvert.DeserializeObject<NoticeVM>(message.Data);

                var result = _notificationService.AddNotification(notice).Result;
                if (result.HasError)
                {
                    _logger.LogError(string.Join(", ", result.ErrorMessages));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }

    }
}
