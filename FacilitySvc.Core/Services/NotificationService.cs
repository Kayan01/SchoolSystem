using Newtonsoft.Json;
using FacilitySvc.Core.Models;
using FacilitySvc.Core.Services.Interfaces;
using FacilitySvc.Core.ViewModels;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.PubSub;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FacilitySvc.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Notice, long> _noticeRepository;
        private readonly IProducerClient<BusMessage> _producerClient;

        public NotificationService(IProducerClient<BusMessage> producerClient, IUnitOfWork unitOfWork,
            IRepository<Notice, long> noticeRepository)
        {
            _unitOfWork = unitOfWork;
            _noticeRepository = noticeRepository;
            _producerClient = producerClient;
        }

        public async Task<ResultModel<object>> GetNotifications()
        {
            var result = new ResultModel<object>();
            result.Data = _noticeRepository.GetAll();
            return result;
        }

        public async Task<ResultModel<NoticeVM>> AddNotification(NoticeVM model)
        {
            var result = new ResultModel<NoticeVM>();
            var test = _noticeRepository.Insert(new Notice { Description = model.Description });
            _unitOfWork.SaveChanges();
            model.Id = test.Id;
            result.Data = model;
            return result;
        }

        public async Task<ResultModel<string>> TestBroadcast(string title)
        {
            var result = new ResultModel<string>();
            var data = new Notice { Description = title };
            await _producerClient.Produce("new_user", new BusMessage
            {
                BusMessageType = (int)BusMessageTypes.NEW_USER,
                Data = JsonConvert.SerializeObject(data)
            });
            result.Data = "Okay";
            return result;
        }
    }
}
