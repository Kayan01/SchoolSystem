using Newtonsoft.Json;
using Auth.Core.Models;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.PubSub;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services
{
    public class TestService : ITestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Test, int> _testRepository;
        private readonly IProducerClient<BusMessage> _producerClient;

        public TestService(IProducerClient<BusMessage> producerClient, IUnitOfWork unitOfWork, IRepository<Test, int> testRepository)
        {
            _unitOfWork = unitOfWork;
            _producerClient = producerClient;
            _testRepository = testRepository;
        }

        public async Task<ResultModel<object>> GetTests()
        {
            var result = new ResultModel<object>();
            result.Data = _testRepository.GetAll();
            return result;
        }

        public async Task<ResultModel<TestVM>> AddTest(TestVM model)
        {
            var result = new ResultModel<TestVM>();
            var test = _testRepository.Insert(new Test { Title = model.Title, Description = model.Description });
            _unitOfWork.SaveChanges();
            model.Id = test.Id;
            result.Data = model;
            return result;
        }

        public async Task<ResultModel<string>> TestBroadcast(string title)
        {
            var result = new ResultModel<string>();
            var data = new Test { Title = title, Description = title };
            await _producerClient.Produce("notice", new BusMessage
            {
                BusMessageType = (int)BusMessageTypes.NOTICE,
                Data = JsonConvert.SerializeObject(data)
            });
            result.Data = "Okay";
            return result;

        }
    }
}
