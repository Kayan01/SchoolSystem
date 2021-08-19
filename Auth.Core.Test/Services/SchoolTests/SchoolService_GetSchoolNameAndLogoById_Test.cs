using Auth.Core.Context;
using Auth.Core.Models;
using Auth.Core.Services.Interfaces;
using Auth.Core.Test.Services.Setup;
using Auth.Core.ViewModels.School;
using NUnit.Framework;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Core.Test.Services.SchoolTests
{
    [TestFixture]
    class SchoolService_GetSchoolNameAndLogoById_Test
    {

        readonly string expectedOutput = "School not found.";
        readonly string logoErrorMessage = "Logo not found.";

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public async Task GetSchoolNameAndLogoById_Test_SchoolInfoNotFound()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = (ISchoolService)_setup.ServiceProvider.GetService(typeof(ISchoolService));
                var context = (AppDbContext)_setup.ServiceProvider.GetService(typeof(AppDbContext));

                context.Schools.Add(new School()
                {
                    DomainName = "Test",
                    Name = "Test School",
                    PrimaryColor = "red",
                    SecondaryColor = "blue"
                });
                await context.SaveChangesAsync();

                var result = await _schoolService.GetSchoolNameAndLogoById(6);

                Assert.That(result.ErrorMessages.Contains(expectedOutput));
            }
        }

        [Test]
        public async Task GetSchoolNameAndLogoById_Test_LogoNotFound()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();
                var context = (AppDbContext)_setup.ServiceProvider.GetService(typeof(AppDbContext));

                context.Schools.Add(new School()
                {
                    DomainName = "Test",
                    Name = "Test School",
                    PrimaryColor = "red",
                    SecondaryColor = "blue"

                });
                await context.SaveChangesAsync();

                

                var result = await _schoolService.GetSchoolNameAndLogoById(4);

                Assert.That(result.ErrorMessages.Contains(logoErrorMessage));
            }
        }

        [Test]
        public async Task GetSchoolNameAndLogoById()
        {

            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = (ISchoolService)_setup.ServiceProvider.GetService(typeof(ISchoolService));
                var context = (AppDbContext)_setup.ServiceProvider.GetService(typeof(AppDbContext));

                var fileUpload = new FileUpload()
                {
                    Name = "Logo",
                    Path = "./file.png",
                    ContentType = "Logo"
                };

                context.FileUploads.Add(fileUpload);

                List<FileUpload> fileUploads = new List<FileUpload>() { fileUpload };

                await context.SaveChangesAsync();

                context.Schools.Add(new School()
                {
                    DomainName = "Test",
                    Name = "Test School",
                    PrimaryColor = "red",
                    SecondaryColor = "blue",
                    FileUploads = fileUploads

                });

                await context.SaveChangesAsync();

                var expectedOutPut = "file";
                
                var result = await _schoolService.GetSchoolNameAndLogoById(4);

                Assert.False(result.HasError);
                Assert.AreEqual(expectedOutPut, result.Data.Logo);
            }
        }

        [Test]
        public async Task GetSchoolNameAndLogoByDomain()
        {
            using(ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                context.Schools.Add(new School()
                {
                    DomainName = "test",
                    Name = "Test School"
                });
                await context.SaveChangesAsync();

                var result = await _schoolService.GetSchoolNameAndLogoByDomain("test");

                Assert.AreEqual("Test School", result.Data.SchoolName);
            }
        }

        [Test]
        public async Task GetSchoolNameAndLogoByDomain_School_Does_Not_Exist()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var result = await _schoolService.GetSchoolNameAndLogoByDomain("test");

                Assert.That(result.ErrorMessages.Contains(expectedOutput));
            }
        }

        [Test]
        public async Task GetSchoolNameAndLogoByDomain_Logo_Not_Found()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();
                context.Schools.Add(new School()
                {
                    DomainName = "test",
                    Name = "Test School"
                });
                await context.SaveChangesAsync();

                var result = await _schoolService.GetSchoolNameAndLogoByDomain("test");

                Assert.That(result.ErrorMessages.Contains(logoErrorMessage));
            }
        }
    }
}
