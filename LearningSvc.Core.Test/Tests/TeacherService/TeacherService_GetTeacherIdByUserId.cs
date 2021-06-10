using NUnit.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Shared.DataAccess.Repository;
using Shared.DataAccess.EfCore.UnitOfWork;
using LearningSvc.Core.Test.Setup;
using LearningSvc.Core.Models;
using LearningSvc.Core.Interfaces;

namespace LearningSvc.Core.Test.Tests.TeacherService
{
    [TestFixture]
    public class TeacherService_GetTeacherIdByUserId
    {
        [Test]
        public async System.Threading.Tasks.Task GetTeachersByUserIdsAsync_AllIdsExist()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _teacherService = _setup.ServiceProvider.GetService<ITeacherService>();
                var context = _setup.DbContext;

                var teacher = new Teacher()
                    {
                        IsActive = true,
                        Email = "teacher1@test.com",
                        Id = 1,
                        UserId = 1,
                    };

                context.Teachers.Add(teacher);
                context.SaveChanges();


                var teacherId = await _teacherService.GetTeacherIdByUserId(1);

                Assert.AreEqual(teacher.Id, teacherId);
            }
        }

        [Test]
        public async System.Threading.Tasks.Task GetTeachersByUserIdsAsync_SomeIdsExist()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _teacherService = _setup.ServiceProvider.GetService<ITeacherService>();
                var context = _setup.DbContext;

                var teacher = new Teacher()
                {
                    IsActive = true,
                    Email = "teacher1@test.com",
                    Id = 1,
                    UserId = 1,
                };

                context.Teachers.Add(teacher);
                context.SaveChanges();


                var teacherId = await _teacherService.GetTeacherIdByUserId(2);

                Assert.AreEqual(0, teacherId);
            }
        }
    }
}
