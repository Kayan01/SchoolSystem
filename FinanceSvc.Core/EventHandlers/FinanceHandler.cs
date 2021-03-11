using Microsoft.Extensions.Logging;
using FinanceSvc.Core.Services.Interfaces;
using Shared.PubSub;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared.ViewModels;
using System.Collections.Generic;

namespace FinanceSvc.Core.EventHandlers
{
    public class FinanceHandler
    {
        private readonly IFinanceService _financeService;
        private readonly ILogger<FinanceHandler> _logger;
        private readonly IStudentService _studentService;
        private readonly IParentService _parentService;
        private readonly ISchoolClassService _schoolClassService;
        private readonly ISessionSetupService _sessionSetupService;

        public FinanceHandler(IFinanceService financeService, 
            ILogger<FinanceHandler> logger,
            IStudentService studentService,
            IParentService parentService,
            ISchoolClassService schoolClassService,
            ISessionSetupService sessionSetupService)
        {
            _financeService = financeService;
            _logger = logger;
            _studentService = studentService;
            _parentService = parentService;
            _schoolClassService = schoolClassService;
            _sessionSetupService = sessionSetupService;
        }

        public async Task HandleAddOrUpdateStudentAsync(BusMessage message)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<StudentSharedModel>(message.Data);
                _studentService.AddOrUpdateStudentFromBroadcast(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }

        public async Task HandleAddOrUpdateParentAsync(BusMessage message)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<ParentSharedModel>(message.Data);
                _parentService.AddOrUpdateParentFromBroadcast(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }

        public async Task HandleAddOrUpdateClassAsync(BusMessage message)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<List<ClassSharedModel>>(message.Data);
                _schoolClassService.AddOrUpdateClassFromBroadcast(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }

        public async Task HandleAddOrUpdateSessionAsync(BusMessage message)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<SessionSharedModel>(message.Data);
                _sessionSetupService.AddOrUpdateSessionFromBroadcast(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }

    }
}
