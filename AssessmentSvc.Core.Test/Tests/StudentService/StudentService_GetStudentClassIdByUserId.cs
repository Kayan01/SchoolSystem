using AssessmentSvc.Core.Test.Setup;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;

namespace AssessmentSvc.Core.Test.Tests.StudentService
{
    [TestFixture]
    public class StudentService_GetStudentClassIdByUserId
    {
        [Test]
        public async System.Threading.Tasks.Task GetStudentClassIdByUserId_StudentExistsAsync()
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

                var classId = await _studentService.GetStudentClassIdByUserId(1);

                Assert.AreEqual(1, classId);
            }
        }

        [Test]
        public async System.Threading.Tasks.Task GetStudentClassIdByUserId_StudentDontExistsAsync()
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

                var classId = await _studentService.GetStudentClassIdByUserId(2);

                Assert.AreNotEqual(1, classId);
                Assert.AreEqual(0, classId);
            }
        }

    }
}
