using AssessmentSvc.Core.Context;
using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.Test.Setup;
using NUnit.Framework;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Test.Services.SchoolTests
{
    [TestFixture]
    public class SchoolService_GetSchool
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public async Task GetSchool()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = (ISchoolService)_setup.ServiceProvider.GetService(typeof(ISchoolService));
                var context = _setup.DbContext;

                var newSchool = new School()
                {
                    Id = 1,
                    Name = "Test",
                    DomainName = "Test",
                    Address = "Test",
                    IsActive = true,
                    WebsiteAddress = "Test.com",
                    City = "Test City",
                    State = "test",
                };

                context.Schools.Add(newSchool);
                context.SaveChanges();

                var schoolCheck = await _schoolService.GetSchool(newSchool.Id);

                Assert.IsFalse(schoolCheck is null);
                Assert.AreEqual(schoolCheck.Name, newSchool.Name);
            }
        }

    }
}
