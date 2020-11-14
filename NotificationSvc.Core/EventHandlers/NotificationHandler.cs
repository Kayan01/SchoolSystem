using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotificationSvc.Core.Services.Interfaces;
using Shared.PubSub;
using Shared.ViewModels;
using System;

namespace NotificationSvc.Core.EventHandlers
{
    public class NotificationHandler
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationHandler> _logger;

        public NotificationHandler(INotificationService noticeService, ILogger<NotificationHandler> logger)
        {
            _notificationService = noticeService;
            _logger = logger;
        }

        public void HandleAddNotification(BusMessage message)
        {
            try
            {
                var notificationMsg = JsonConvert.DeserializeObject<CreateNotificationModel>(message.Data);

                var result = _notificationService.CreateNotification(notificationMsg).Result;
                if (result.HasError)
                {
                    _logger.LogError(string.Join(", ", result.ErrorMessages));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }

    }
}
