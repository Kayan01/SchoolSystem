using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels;
using Shared.PubSub;
using System;
using Shared.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using LearningSvc.Core.Models;
using Shared.DataAccess.Repository;
using Shared.DataAccess.EfCore.UnitOfWork;
using System.Linq;
using LearningSvc.Core.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LearningSvc.Core.EventHandlers
{
    public class LearningHandler
    {
        private readonly ILogger<LearningHandler> _logger;
        private readonly ISchoolClassService _schoolClassService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;

        public LearningHandler(ILogger<LearningHandler> logger,
            ISchoolClassService schoolClassService,
            IStudentService studentService,
            ITeacherService teacherService)
        {
            _logger = logger;
            _studentService = studentService;
            _teacherService = teacherService;
            _schoolClassService = schoolClassService;
        }

        public async Task HandleAddOrUpdateStudentAsync(BusMessage message)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<StudentSharedModel>(message.Data);
                await _studentService.AddOrUpdateStudentFromBroadcast(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }

        public async Task HandleAddOrUpdateTeacherAsync(BusMessage message)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<TeacherSharedModel>(message.Data);
                await _teacherService.AddOrUpdateTeacherFromBroadcast(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }

        public async Task HandleAddOrUpdateClassAsync(BusMessage message)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<ClassSharedModel>(message.Data);
                await _schoolClassService.AddOrUpdateClassFromBroadcast(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }
        public async Task HandleAddOrUpdateClassRangeAsync(BusMessage message)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<List<ClassSharedModel>>(message.Data);
                await _schoolClassService.AddOrUpdateClassRangeFromBroadcast(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }


    }
}
