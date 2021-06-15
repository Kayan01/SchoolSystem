using AssessmentSvc.Core.Context;
using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.Test.Setup;
using NUnit.Framework;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Test.Tests.TeacherService
{
    [TestFixture]
    public class TeacherService_AddOrUpdateTeacherFromBroadcast
    {
        [SetUp]
        public void SetUp()
        {
        }


        [Test]
        public void AddOrUpdateTeacherFromBroadcast_AddTeacher()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _teacherService = (ITeacherService)_setup.ServiceProvider.GetService(typeof(ITeacherService));
                var context = _setup.DbContext;

                var newTeacher = new TeacherSharedModel()
                {
                    IsActive = true,
                    Email = "o@test.com",
                    Id = 1,
                };

                _teacherService.AddOrUpdateTeacherFromBroadcast(newTeacher);

                var teacherCheck = context.Teachers.SingleOrDefault(x => x.Id == newTeacher.Id);

                Assert.IsNotNull(teacherCheck);
                Assert.AreEqual(newTeacher.Email, teacherCheck.Email);
            }
        }

        [Test]
        public void AddOrUpdateTeacherFromBroadcast_UpdateTeacher()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _teacherService = (ITeacherService)_setup.ServiceProvider.GetService(typeof(ITeacherService));
                var teacherRepo = (IRepository<Teacher, long>)_setup.ServiceProvider.GetService(typeof(IRepository<Teacher, long>));
                var _unitOfWork = (IUnitOfWork)_setup.ServiceProvider.GetService(typeof(IUnitOfWork));


                var oldTeacher = new Teacher()
                {
                    IsActive = true,
                    Email = "old@test.com",
                    Id = 1,
                };
                teacherRepo.Insert(oldTeacher);
                _unitOfWork.SaveChanges();

                var newTeacher = new TeacherSharedModel()
                {
                    IsActive = true,
                    Email = "new@test.com",
                    Id = 1,
                };

                _teacherService.AddOrUpdateTeacherFromBroadcast(newTeacher);

                var teacherChecks = teacherRepo.GetAll().ToList();
                var teacherCheck = teacherRepo.GetAll().SingleOrDefault(x => x.Id == newTeacher.Id);

                Assert.IsNotNull(teacherCheck);
                Assert.AreEqual(newTeacher.Email, teacherCheck.Email);
            }
        }

    }
}
