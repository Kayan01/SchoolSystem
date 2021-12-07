using IPagedList;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NotificationSvc.Core.Context;
using NotificationSvc.Core.Models;
using NotificationSvc.Core.Services.Interfaces;
using NotificationSvc.Core.ViewModels;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationSvc.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Notification, long> _noticeRepository;
        private readonly IEmailService _emailService;

        public NotificationService(IUnitOfWork unitOfWork,
            IEmailService emailService,
            IRepository<Notification, long> noticeRepository)
        {
            _unitOfWork = unitOfWork;
            _noticeRepository = noticeRepository;
            _emailService = emailService;
        }

        public async Task<ResultModel<PaginatedModel<NotificationVM>>> GetNotifications(PagedRequestModel model, long userId)
        {
            var query = _noticeRepository.GetAllIncluding(x => x.UserNotifications)
                                           .Where(x => x.UserNotifications.Any(x => x.UserId == userId));

            var pagedData = await query.ToPagedListAsync(model.PageIndex, model.PageSize);

            return new ResultModel<PaginatedModel<NotificationVM>>(new PaginatedModel<NotificationVM>(pagedData.Select(x => (NotificationVM)x), model.PageIndex, model.PageSize, pagedData.TotalItemCount), "Success");
        }

        public async Task<ResultModel<List<NotificationVM>>> CreateNotification(CreateNotificationModel model)
        {
            await SendEmailNotification(model.Emails);

            var result = new List<Notification>();

            foreach(var notificationMsg in model.Notifications)
            {
                var notification = new Notification
                {
                    Entity = notificationMsg.Entity,
                    EntityId = notificationMsg.EntityId,
                    Description = notificationMsg.Description
                };
                notification.UserNotifications = notificationMsg.UserIds.Select(x => new UserNotification { UserId = x}).ToList();

                result.Add(_noticeRepository.Insert(notification));
            }
            _unitOfWork.SaveChanges();

            return new ResultModel<List<NotificationVM>>(result.Select(x => (NotificationVM)x).ToList(), "Success");
        }

        private async Task SendEmailNotification(List<CreateEmailModel> emailMessages)
        {
            foreach (var emailMessage in emailMessages)
            {
                //emailMessage.ReplacementData.Add("FullName", emailMessage.User.FullName);
                await _emailService.SendEmail(new[] { emailMessage.User.Email }, emailMessage.EmailTemplateType, emailMessage.ReplacementData,emailMessage.SenderName, emailMessage.EmailPassword,emailMessage.Attachments);
            }
        }

        public async Task<ResultModel<List<NotificationVM>>> ReadNotification(long[] notificationId, long userId)
        {
            var notifications = _noticeRepository.GetAll().Where(x => notificationId.Contains(x.Id) && x.UserNotifications.Any(x => x.UserId == userId));

            notifications.ToList().ForEach(x => x.UserNotifications.ToList().ForEach(y => y.IsRead = true));

            _unitOfWork.SaveChanges();

            return new ResultModel<List<NotificationVM>>(notifications.Select(x => (NotificationVM)x).ToList(), "Success");
        }
    }
}
