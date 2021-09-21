using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using LearningSvc.Core.Interfaces;
using Shared.PubSub;
using System;
using Shared.ViewModels;
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

        private readonly ISchoolService _schoolService;
        private readonly IParentService _parentService;
        public LearningHandler(ILogger<LearningHandler> logger,
            ISchoolClassService schoolClassService,
            IStudentService studentService, 
            ISchoolService schoolService,
            ITeacherService teacherService, 
            IParentService parentService)
        {
            _logger = logger;
            _studentService = studentService;
            _teacherService = teacherService;
            _schoolClassService = schoolClassService;
            _parentService = parentService;
            _schoolService = schoolService; 
        }

        public async Task HandleAddOrUpdateStudentAsync(BusMessage message)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<List<StudentSharedModel>>(message.Data);
                _studentService.AddOrUpdateStudentFromBroadcast(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }

        public async Task HandleAddOrUpdateTeacherAsync(BusMessage message)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<TeacherSharedModel>(message.Data);
                _teacherService.AddOrUpdateTeacherFromBroadcast(data);
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
        public async Task HandleAddOrUpdateSchoolAsync(BusMessage message)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<SchoolSharedModel>(message.Data);
                _schoolService.AddOrUpdateSchoolFromBroadcast(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }

    }
}
