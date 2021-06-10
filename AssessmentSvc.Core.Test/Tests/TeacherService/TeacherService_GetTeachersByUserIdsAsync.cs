using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Test.Setup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using AssessmentSvc.Core.Models;
using Shared.DataAccess.Repository;
using Shared.DataAccess.EfCore.UnitOfWork;

namespace AssessmentSvc.Core.Test.Tests.TeacherService
{
    [TestFixture]
    public class TeacherService_GetTeachersByUserIdsAsync
    {
        [Test]
        public async System.Threading.Tasks.Task GetTeachersByUserIdsAsync_AllIdsExist()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _teacherService = _setup.ServiceProvider.GetService<ITeacherService>();
                var _teacherRepo = _setup.ServiceProvider.GetService<IRepository<Teacher, long>>();
                var _unitOfWork = _setup.ServiceProvider.GetService<IUnitOfWork>();

                var teachers = new List<Teacher>
                {
                    new Teacher()
                    {
                        IsActive = true,
                        Email = "teacher1@test.com",
                        Id = 1,
                        UserId = 1,
                    }, new Teacher()
                    {
                        IsActive = true,
                        Email = "teacher2@test.com",
                        Id = 2,
                        UserId = 2,
                    }, new Teacher()
                    {
                        IsActive = true,
                        Email = "teacher3@test.com",
                        Id = 3,
                        UserId = 3,
                    }
                }; 
                
                foreach (var item in teachers)
                {
                    _teacherRepo.Insert(item);
                }

                _unitOfWork.SaveChanges();


                var teachersCheck = await _teacherService.GetTeachersByUserIdsAsync(new List<long> { 1, 2 });

                Assert.IsNotNull(teachersCheck);
                Assert.AreEqual(2, teachersCheck.Count);


                Assert.AreEqual(teachers[0].Id, teachersCheck[0].Id);
                Assert.AreEqual(teachers[1].Id, teachersCheck[1].Id);
            }
        }

        [Test]
        public async System.Threading.Tasks.Task GetTeachersByUserIdsAsync_SomeIdsExist()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _teacherService = _setup.ServiceProvider.GetService<ITeacherService>();
                var context = _setup.DbContext;

                var teachers = new List<Teacher>
                {
                    new Teacher()
                    {
                        IsActive = true,
                        Email = "teacher1@test.com",
                        Id = 1,
                        UserId = 1,
                    }, new Teacher()
                    {
                        IsActive = true,
                        Email = "teacher2@test.com",
                        Id = 2,
                        UserId = 2,
                    }, new Teacher()
                    {
                        IsActive = true,
                        Email = "teacher3@test.com",
                        Id = 3,
                        UserId = 3,
                    }
                };

                context.Teachers.AddRange(teachers);
                context.SaveChanges();

                var teachersCheck = await _teacherService.GetTeachersByUserIdsAsync(new List<long> { 1, 4 });

                Assert.IsNotNull(teachersCheck);
                Assert.AreEqual(1, teachersCheck.Count);


                Assert.AreEqual(teachers[0].Id, teachersCheck[0].Id);
            }
        }
    }
}
