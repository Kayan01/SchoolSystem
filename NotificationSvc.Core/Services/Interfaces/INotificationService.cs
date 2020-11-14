using NotificationSvc.Core.ViewModels;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSvc.Core.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ResultModel<PaginatedModel<NotificationVM>>> GetNotifications(PagedRequestModel model, long userId);
        Task<ResultModel<List<NotificationVM>>> CreateNotification(CreateNotificationModel model);
        Task<ResultModel<List<NotificationVM>>> ReadNotification(long[] notificationId, long userId);
    }
}
