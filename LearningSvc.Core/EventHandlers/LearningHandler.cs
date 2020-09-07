using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using LearningSvc.Core.Services.Interfaces;
using LearningSvc.Core.ViewModels;
using Shared.PubSub;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.EventHandlers
{
    public class LearningHandler
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<LearningHandler> _logger;

        public LearningHandler(INotificationService noticeService, ILogger<LearningHandler> logger)
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
                    //throw new Exception(string.Join(",", result.Select(x => x.ErrorMessage)));
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
