using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Test.Setup;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Shared.ViewModels;
using System.Linq;
using AssessmentSvc.Core.Models;

namespace AssessmentSvc.Core.Test.Tests.StudentService
{
    [TestFixture]
    public class StudentService_AddOrUpdateStudentFromBroadcast
    {
        [Test]
        public void AddOrUpdateStudentFromBroadcast_AddStudent()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _studentService = _setup.ServiceProvider.GetService<IStudentService>();
                var context = _setup.DbContext;

                var newStudent = new StudentSharedModel()
                {
                    IsActive = true,
                    FirstName = "firstname",
                    Id = 1,
                };

                _studentService.AddOrUpdateStudentFromBroadcast(new List<StudentSharedModel> { newStudent });

                var studentCheck = context.Students.SingleOrDefault(x => x.Id == newStudent.Id);

                Assert.IsNotNull(studentCheck);
                Assert.AreEqual(newStudent.FirstName, studentCheck.FirstName);
            }
        }

        [Test]
        public void AddOrUpdateStudentFromBroadcast_UpdateStudent()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _studentService = (IStudentService)_setup.ServiceProvider.GetService(typeof(IStudentService));
                var context = _setup.DbContext;


                var oldStudent = new Student()
                {
                    IsActive = true,
                    FirstName = "oldstudent",
                    Id = 1,
                };
                context.Add(oldStudent);
                context.SaveChanges();

                var newStudent = new StudentSharedModel()
                {
                    IsActive = true,
                    FirstName = "newstudent",
                    Id = 1,
                };

                _studentService.AddOrUpdateStudentFromBroadcast(new List<StudentSharedModel> { newStudent });

                var studentCheck = context.Students.SingleOrDefault(x => x.Id == newStudent.Id);

                Assert.IsNotNull(studentCheck);
                Assert.AreEqual(newStudent.FirstName, studentCheck.FirstName);
            }
        }
    }
}
