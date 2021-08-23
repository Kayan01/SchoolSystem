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
using Auth.Core.Models.Contact;
using Auth.Core.ViewModels;

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
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

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
                    DomainName = "test",
                    Name = "Test School",
                    PrimaryColor = "red",
                    SecondaryColor = "blue",
                    FileUploads = fileUploads

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
                    Id = 4,
                    DomainName = "test",
                    Name = "Test School"
                });
                await context.SaveChangesAsync();

                var result = await _schoolService.GetSchoolNameAndLogoByDomain("test");

                Assert.That(result.ErrorMessages.Contains(logoErrorMessage));
            }
        }

        [Test]
        public async Task GetSchoolById()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var contactDetails = new SchoolContactDetails()
                {
                    IsPrimaryContact = true,
                    FirstName = "Ade",
                    LastName = "Ola",
                    Email = "A@gmail.com"
                };
                context.Add(contactDetails);
                List<SchoolContactDetails> ListOfContactDetails = new List<SchoolContactDetails>();
                ListOfContactDetails.Add(contactDetails);

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
                    DomainName = "test",
                    Name = "Test School",
                    PrimaryColor = "red",
                    SecondaryColor = "blue",
                    FileUploads = fileUploads,
                    SchoolContactDetails = ListOfContactDetails,
                });

                await context.SaveChangesAsync();

                var result = await _schoolService.GetSchoolById(4);

                Assert.That(result.ErrorMessages.Count == 0);
                Assert.AreEqual("test", result.Data.DomainName);
            }
        }

        [Test]
        public async Task UpdateSchool()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var contactDetails = new SchoolContactDetails()
                {
                    IsPrimaryContact = true,
                    FirstName = "Ade",
                    LastName = "Ola",
                    Email = "A@gmail.com"
                };
                context.Add(contactDetails);

                List<SchoolContactDetails> ListOfContactDetails = new List<SchoolContactDetails>();
                ListOfContactDetails.Add(contactDetails);

                var fileUpload = new FileUpload()
                {
                    Name = "Logo",
                    Path = "./file.png",
                    ContentType = "Logo"
                };
                context.FileUploads.Add(fileUpload);

                List<FileUpload> fileUploads = new List<FileUpload>() { fileUpload };

                await context.SaveChangesAsync();

                var school = new School()
                {
                    DomainName = "tested",
                    Name = "Test School",
                    PrimaryColor = "red",
                    SecondaryColor = "blue",
                    FileUploads = fileUploads,
                    SchoolContactDetails = ListOfContactDetails,
                };
                context.Schools.Add(school);
                context.SaveChanges();

                var Data = await UpdateModelData();

                var result = await _schoolService.UpdateSchool(Data, 4);

                Assert.That(!result.ErrorMessages.Contains("User does not exist"));
                Assert.AreEqual(Data.DomainName, result.Data.Name);
            }
        }

        [Test]
        public async Task UpdateSchool_Domain_Already_Exist()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();

                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var contactDetails = new SchoolContactDetails()
                {
                    IsPrimaryContact = true,
                    FirstName = "Ade",
                    LastName = "Ola",
                    Email = "A@gmail.com"
                };
                context.Add(contactDetails);

                List<SchoolContactDetails> ListOfContactDetails = new List<SchoolContactDetails>();
                ListOfContactDetails.Add(contactDetails);

                var fileUpload = new FileUpload()
                {
                    Name = "Logo",
                    Path = "./file.png",
                    ContentType = "Logo"
                };
                context.FileUploads.Add(fileUpload);

                List<FileUpload> fileUploads = new List<FileUpload>() { fileUpload };

                var school = new School()
                {
                    DomainName = "test",
                    Name = "Test School",
                    PrimaryColor = "red",
                    SecondaryColor = "blue",
                    FileUploads = fileUploads,
                    SchoolContactDetails = ListOfContactDetails,
                };
                context.Schools.Add(school);
                await context.SaveChangesAsync();

               // var testData = await TestData();

                var Data = await UpdateModelData();

                var result = await _schoolService.UpdateSchool(Data, 4);

                Assert.That(result.ErrorMessages.Contains("Unique name required for domain"));
            }
        }

        [Test]
        public async Task UpdateSchool_School_Does_Not_Exist()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var Data = await UpdateModelData();

                var result = await _schoolService.UpdateSchool(Data, 4);
                Assert.That(result.ErrorMessages.Contains("School does not exist"));
            }
        }

        [Test]
        public async Task UpdateSchool_User_Does_Not_exist()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var contactDetails = new SchoolContactDetails()
                {
                    IsPrimaryContact = true,
                    FirstName = "Ade",
                    LastName = "Ola",
                    Email = "A@gmail.com"
                };
                context.Add(contactDetails);

                List<SchoolContactDetails> ListOfContactDetails = new List<SchoolContactDetails>();
                ListOfContactDetails.Add(contactDetails);

                var fileUpload = new FileUpload()
                {
                    Name = "Logo",
                    Path = "./file.png",
                    ContentType = "Logo"
                };
                context.FileUploads.Add(fileUpload);

                List<FileUpload> fileUploads = new List<FileUpload>() { fileUpload };

                await context.SaveChangesAsync();

                var school = new School()
                {
                    DomainName = "tested",
                    Name = "Test School",
                    PrimaryColor = "red",
                    SecondaryColor = "blue",
                    FileUploads = fileUploads,
                    SchoolContactDetails = ListOfContactDetails,
                };
                context.Schools.Add(school);
                context.SaveChanges();

                var Data = await UpdateModelData();

                var result = await _schoolService.UpdateSchool(Data, 4);

                Assert.That(result.ErrorMessages.Contains("User does not exist"));
            }
        }

        private async Task<UpdateSchoolVM> UpdateModelData()
        {
            var Data = new UpdateSchoolVM()
            {
                DomainName = "test",
                Name = "Test School",
                PrimaryColor = "red",
                SecondaryColor = "blue",
                WebsiteAddress = "W.AspNet.com",
                Address = "Address1",
                City = "Lekki",
                State = "Lagos",
                Country = "Nigeria",
                ContactFirstName = "Ade",
                ContactLastName = "OLa",
                ContactPhoneNo = "090567564532",
                ContactEmail = "Bed@gmail.com",
                //Username = "tester@gmail.com", //available User
                Username = "User", //User not in database
                IsActive = true
            };

            return Data;
        }
    }
}
