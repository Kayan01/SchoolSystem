using AssessmentSvc.Core.Context;
using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Test.Setup;
using NUnit.Framework;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AssessmentSvc.Core.Test.Tests.SchoolService
{
    [TestFixture]
    public class SchoolService_AddOrUpdateSchool
    {
        [SetUp]
        public void SetUp()
        {
        }


        [Test]
        public async Task AddOrUpdateSchoolFromBroadcast_AddSchool()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();
                var context = _setup.DbContext;

                var newSchool = new SchoolSharedModel()
                {
                    Name = "TestAddSchool",
                    DomainName = "Test",
                    Address = "Test",
                    IsActive = true,
                    WebsiteAddress = "Test.com",
                    City = "Test City",
                    Email = "o@test.com",
                    Id = 1,
                    Logo = "test.png",
                    PhoneNumber = "09090000999",
                    State = "test",
                };

                _schoolService.AddOrUpdateSchoolFromBroadcast(newSchool);

                var schoolCheck = context.Schools.SingleOrDefault(x => x.Id == newSchool.Id);

                Assert.IsFalse(schoolCheck is null);
                Assert.AreEqual(schoolCheck.Name, newSchool.Name);
            }
        }

        [Test]
        public async Task AddOrUpdateSchoolFromBroadcast_UpdateSchool()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = (ISchoolService)_setup.ServiceProvider.GetService(typeof(ISchoolService));
                var context = _setup.DbContext;

                var oldSchool = new SchoolSharedModel()
                {
                    Name = "OldTestSchool",
                    DomainName = "Test",
                    Address = "Test",
                    IsActive = true,
                    WebsiteAddress = "Test.com",
                    City = "Test City",
                    Email = "o@test.com",
                    Id = 1,
                    Logo = "test.png",
                    PhoneNumber = "09090000999",
                    State = "test",
                };

                await context.SaveChangesAsync();

                var newSchool = new SchoolSharedModel()
                {
                    Name = "NewTestSchool",
                    DomainName = "Test",
                    Address = "Test",
                    IsActive = true,
                    WebsiteAddress = "Test.com",
                    City = "Test City",
                    Email = "o@test.com",
                    Id = 1,
                    Logo = "test.png",
                    PhoneNumber = "09090000999",
                    State = "test",
                };

                _schoolService.AddOrUpdateSchoolFromBroadcast(newSchool);

                var schoolCheck = context.Schools.SingleOrDefault(x => x.Id == newSchool.Id);

                Assert.IsFalse(schoolCheck is null);
                Assert.AreEqual(schoolCheck.Name, newSchool.Name);
            }
        }

    }
}
