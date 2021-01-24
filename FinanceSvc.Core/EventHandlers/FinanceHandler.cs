using Microsoft.Extensions.Logging;
using FinanceSvc.Core.Services.Interfaces;
using Shared.PubSub;
using System;

namespace FinanceSvc.Core.EventHandlers
{
    public class FinanceHandler
    {
        private readonly IFinanceService _financeService;
        private readonly ILogger<FinanceHandler> _logger;

        public FinanceHandler(IFinanceService financeService, ILogger<FinanceHandler> logger)
        {
            _financeService = financeService;
            _logger = logger;
        }

        public void HandleTest(BusMessage message)
        {
            throw new NotImplementedException();
        }

    }
}
