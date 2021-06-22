using Moq;
using Shared.PubSub;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Test.Mocks
{
    public class MockPublishService
    {
        public Mock<IPublishService> Mock { get; } 

        public MockPublishService()
        {
            Mock = new Mock<IPublishService>();

            Mock.Setup(m =>  m.PublishMessage(It.IsAny<string>(), It.IsAny<BusMessageTypes>(), It.IsAny<object>())).Returns( Task.CompletedTask );
        }
    }
}
