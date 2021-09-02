using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Shared.PubSub;
using System;
using System.Collections.Generic;
using System.Text;
using Shared.ViewModels;
using Auth.Core.Interfaces;
using System.Threading.Tasks;

namespace Auth.Core.EventHandlers
{
    public class AuthHandler
    {
        private readonly ITestService _testService;
        private readonly IPromotionService _promotionService;
        private readonly ILogger<AuthHandler> _logger;

        public AuthHandler(ITestService testService,
            IPromotionService promotionService,
            ILogger<AuthHandler> logger)
        {
            _testService = testService;
            _promotionService = promotionService;
            _logger = logger;
        }

        public void HandleTest(BusMessage message)
        {
            try
            {
                var test = JsonConvert.DeserializeObject<TestVM>(message.Data);

                var result = _testService.AddTest(test).Result;
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

        public async Task HandlePromotionAsync(BusMessage message)
        {
            try
            {
                var promoData = JsonConvert.DeserializeObject<PromotionSharedModel>(message.Data);
                await _promotionService.PromoteAllStudent(promoData);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }

    }
}
