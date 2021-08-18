using Auth.Core.Context;
using Auth.Core.Models;
using Auth.Core.Services;
using Auth.Core.Services.Interfaces;
using Auth.Core.Test.Services.Setup;
using Auth.Core.ViewModels.School;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Entities;
using Shared.FileStorage;
using Shared.PubSub;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Test.Services.SchoolTests
{
    [TestFixture]
    public class SchoolService_AddSchool_Test
    {

        public const string DomainErrorMessage = "Unique name required for domain";

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public async Task AddSchool_DomainExistsAsync()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = (ISchoolService)_setup.ServiceProvider.GetService(typeof(ISchoolService));
                var context = (AppDbContext)_setup.ServiceProvider.GetService(typeof(AppDbContext));

                context.Schools.Add(new School()
                {
                    DomainName = "Test",
                    Name = "Test School"
                });
                await context.SaveChangesAsync();

                var result = await _schoolService.CheckSchoolDomain(new CreateSchoolVM()
                {
                    Name = "Test",
                    DomainName = "Test",
                });


                Assert.That(result.ErrorMessages.Contains(DomainErrorMessage));
            }
        }


        [Test]
        public async Task AddSchool_Domain_Does_Not_ExistsAsync()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = (ISchoolService)_setup.ServiceProvider.GetService(typeof(ISchoolService));
                var context = (AppDbContext)_setup.ServiceProvider.GetService(typeof(AppDbContext));

                context.Schools.Add(new School()
                {
                    DomainName = "Test",
                    Name = "Test School"
                });
                await context.SaveChangesAsync();

                var result = await _schoolService.CheckSchoolDomain(new CreateSchoolVM()
                {
                    Name = "Test",
                    DomainName = "Test 2",
                });


                Assert.That(!result.ErrorMessages.Contains(DomainErrorMessage));
            }
        }

        [Test]
        public async Task AddSchoolAsync()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = (ISchoolService)_setup.ServiceProvider.GetService(typeof(ISchoolService));
                var result = await _schoolService.AddSchool(new CreateSchoolVM()
                {
                    Name = "Test",
                    DomainName = "Test",
                    Address = "Test",
                    IsActive = true,
                    ContactEmail = "a@e.com",
                    WebsiteAddress = "Test.com",
                    City = "Test City",
                    ContactFirstName = "Test",
                    ContactLastName = "Name",
                    ContactPhoneNo = "09090000999",
                    Country = "Nigeria",
                    Username = "test",
                    State = "test",
                });

                Assert.That(!result.ErrorMessages.Contains(DomainErrorMessage));
            }
        }
    }
}
