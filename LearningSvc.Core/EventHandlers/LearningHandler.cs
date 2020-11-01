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

namespace LearningSvc.Core.EventHandlers
{
    public class LearningHandler
    {
        private readonly ILogger<LearningHandler> _logger;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IRepository<Teacher, long> _teacherRepo;
        private readonly IUnitOfWork _unitOfWork;

        public LearningHandler(ILogger<LearningHandler> logger,
            IRepository<Student, long> studentRepo,
            IRepository<Teacher, long> teacherRepo,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _studentRepo = studentRepo;
            _teacherRepo = teacherRepo;
        }

        public void HandleAddOrUpdateStudent(BusMessage message)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<StudentSharedModel>(message.Data);

                var student = _studentRepo.FirstOrDefault(x => x.Id == data.Id && x.TenantId == data.TenantId);
                if(student == null)
                {
                    student = _studentRepo.Insert(new Student
                    {
                        Id = data.Id
                    });
                }

                student.TenantId = data.TenantId;
                student.ClassId = data.ClassId;
                student.FirstName = data.FirstName;
                student.LastName = data.LastName;
                student.Email = data.Email;
                student.Phone = data.Phone;
                student.UserId = data.UserId;
                student.IsActive = data.IsActive;
                student.IsDeleted = data.IsDeleted;

                _unitOfWork.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }

        public void HandleAddOrUpdateTeacher(BusMessage message)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<TeacherSharedModel>(message.Data);

                var teacher = _teacherRepo.FirstOrDefault(x => x.Id == data.Id && x.TenantId == data.TenantId);
                if (teacher == null)
                {
                    teacher = _teacherRepo.Insert(new Teacher
                    {
                        Id = data.Id
                    });
                }

                teacher.TenantId = data.TenantId;
                teacher.ClassId = data.ClassId;
                teacher.FirstName = data.FirstName;
                teacher.LastName = data.LastName;
                teacher.Email = data.Email;
                teacher.Phone = data.Phone;
                teacher.UserId = data.UserId;
                teacher.IsActive = data.IsActive;
                teacher.IsDeleted = data.IsDeleted;

                _unitOfWork.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }

    }
}
