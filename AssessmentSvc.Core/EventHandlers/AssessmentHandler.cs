using AssessmentSvc.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.PubSub;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.EventHandlers
{
    public class AssessmentHandler
    {

        private readonly ILogger<AssessmentHandler> _logger;
        private readonly ISchoolClassService _schoolClassService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly ISubjectService _subjectService;

        public AssessmentHandler(ILogger<AssessmentHandler> logger,
            ISchoolClassService schoolClassService,
            IStudentService studentService,
            ITeacherService teacherService,
            ISubjectService subjectService
            )
        {
            _logger = logger;
            _studentService = studentService;
            _teacherService = teacherService;
            _schoolClassService = schoolClassService;
            _subjectService = subjectService;
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

        public async Task HandleAddOrUpdateSubjectAsync(BusMessage message)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<SubjectSharedModel>(message.Data);
                _subjectService.AddOrUpdateSubjectFromBroadcast(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }
    }
}
