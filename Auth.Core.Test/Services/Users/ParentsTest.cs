using Auth.Core.Test.Services.Setup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Auth.Core.Interfaces.Users;
using Auth.Core.ViewModels.Parent;
using Shared.Enums;
using Auth.Core.Interfaces.Setup;
using Auth.Core.ViewModels.Setup;
using Auth.Core.Context;

namespace Auth.Core.Test.Services.Users
{
    [TestFixture]
    class ParentsTest
    {
        [SetUp]
        public void Setup()
        { 
        }

        [Test]
        public async Task AddParent_Test()
        {
            using ServicesDISetup _setup = new ServicesDISetup();
            var ParentService = _setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var context = _setup.ServiceProvider.GetService<AppDbContext>();

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var data = AddParentVMData();

            var result = await ParentService.AddNewParent(data);

            Assert.That(result.ErrorMessages.Count == 0);
            Assert.AreEqual(result.Data.ContactEmail, data.EmailAddress);
        }

        private AddParentVM AddParentVMData()
        {
            var data = new AddParentVM()
            {
                Title = "Mr",
                FirstName = "Oke",
                LastName = "Ekene",
                Sex = "Male",
                Occupation = "Doctor",
                ModeOfIdentification = "NIN",
                IdentificationNumber = "9788892718",
                PhoneNumber = "09087675645",
                EmailAddress = "bued@gmail.com",
                HomeAddress = "7 kunle lawal",
                OfficeAddress = "17h karimu kolawate",
                DocumentType = DocumentType.Logo
            };

            return data;
        }
        private SchoolPropertyVM SchoolPropertyVMData()
        {
            var data = new SchoolPropertyVM()
            {
                Prefix = "GA",
                Seperator = ":",
                EnrollmentAmount = 100000,
                NumberOfTerms = 3,
                ClassDays = ClassDaysType.WeekDaysOnly
            };
            return data;
        }
    }
}
