using LearningSvc.Core.Interfaces;
using LearningSvc.Core.Test.Setup;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using LearningSvc.Core.Models;

namespace LearningSvc.Core.Test.Tests.StudentService
{
    [TestFixture]
    public class StudentService_GetStudentIdByUserId
    {
        [Test]
        public async System.Threading.Tasks.Task GetStudentIdByUserId_StudentExistsAsync()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _studentService = _setup.ServiceProvider.GetService<IStudentService>();
                var context = _setup.DbContext;

                var student = new Student()
                {
                    IsActive = true,
                    FirstName = "firstname",
                    Id = 2,
                    UserId = 1,
                    Class = new SchoolClass()
                    {
                        Id = 1,
                        ClassArm = "primary",
                        Name = "SS1"
                    }
                };

                context.Students.Add(student);
                context.SaveChanges();

                var classId = await _studentService.GetStudentIdByUserId(1);

                Assert.AreEqual(2, classId);
            }
        }

        [Test]
        public async System.Threading.Tasks.Task GetStudentIdByUserId_StudentDontExistsAsync()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _studentService = _setup.ServiceProvider.GetService<IStudentService>();
                var context = _setup.DbContext;

                var student = new Student()
                {
                    IsActive = true,
                    FirstName = "firstname",
                    Id = 1,
                    UserId = 1,
                    Class = new SchoolClass()
                    {
                        Id = 1,
                        ClassArm = "primary",
                        Name = "SS1"
                    }
                };

                context.Students.Add(student);
                context.SaveChanges();

                var classId = await _studentService.GetStudentIdByUserId(2);

                Assert.AreNotEqual(1, classId);
                Assert.AreEqual(0, classId);
            }
        }

    }
}
