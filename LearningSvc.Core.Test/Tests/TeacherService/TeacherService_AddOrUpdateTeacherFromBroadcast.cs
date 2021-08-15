using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningSvc.Core.Test.Setup;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.Models;

namespace LearningSvc.Core.Test.Tests.TeacherService
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
                var _teacherService = _setup.ServiceProvider.GetService<ITeacherService>();
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
                var _teacherService = _setup.ServiceProvider.GetService<ITeacherService>();
                var context = _setup.DbContext;


                var oldTeacher = new Teacher()
                {
                    IsActive = true,
                    Email = "old@test.com",
                    Id = 1,
                };
                context.Add(oldTeacher);
                context.SaveChanges();

                var newTeacher = new TeacherSharedModel()
                {
                    IsActive = true,
                    Email = "new@test.com",
                    Id = 1,
                };

                _teacherService.AddOrUpdateTeacherFromBroadcast(newTeacher);

                var teacherCheck = context.Teachers.SingleOrDefault(x => x.Id == newTeacher.Id);

                Assert.IsNotNull(teacherCheck);
                Assert.AreEqual(newTeacher.Email, teacherCheck.Email);
            }
        }

    }
}
