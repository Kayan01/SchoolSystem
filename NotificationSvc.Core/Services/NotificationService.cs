using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NotificationSvc.Core.Context;
using NotificationSvc.Core.Models;
using NotificationSvc.Core.Services.Interfaces;
using NotificationSvc.Core.ViewModels;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.PubSub;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSvc.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext appDbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Notice, long> _noticeRepository;
        private readonly IProducerClient<BusMessage> _producerClient;
        private readonly IRepository<TestModel, long> _testRepository;

        public NotificationService(IProducerClient<BusMessage> producerClient, IUnitOfWork unitOfWork,
            IRepository<Notice, long> noticeRepository, AppDbContext _appDbContext, IRepository<TestModel, long> testRepository)
        {
            _unitOfWork = unitOfWork;
            _noticeRepository = noticeRepository;
            _producerClient = producerClient;
            appDbContext = _appDbContext;
            _testRepository = testRepository;
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
            result.Data = "Successful";
            return result;
        }

        public async Task<ResultModel<object>> GetTestModelsWithTenants()
        {
            var result = new ResultModel<object>();
            //   await appDbContext.AddSampleData();


            //uncomment to add new entity to test tenancy
          //  _testRepository.Insert(new TestModel { Name = "John Innocent" });
          // await _unitOfWork.SaveChangesAsync();
            var t = await _testRepository.GetAllListAsync();

            result.Data = t;

            return result;
        
        }
    }
}
