using NotificationSvc.Core.ViewModels;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSvc.Core.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ResultModel<object>> GetNotifications();
        Task<ResultModel<string>> TestBroadcast(string title);
        Task<ResultModel<NoticeVM>> AddNotification(NoticeVM model);
    }
}
