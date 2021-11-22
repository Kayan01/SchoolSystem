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

        [Test]
        public async Task UpdateParent_Test()
        {
            using ServicesDISetup _setup = new ServicesDISetup();
            var parentService = _setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var context = _setup.ServiceProvider.GetService<AppDbContext>();

            var parentData = AddParentVMData();
            var updateData = UpdateParentData();
            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            var AddParent = await parentService.AddNewParent(parentData);

            var result = await parentService.UpdateParent(1,updateData);

            Assert.That(result.ErrorMessages.Count == 0);
            Assert.AreEqual(updateData.EmailAddress, result.Data.ContactEmail);
        }

        [Test]
        public async Task UpdateParent_Id_Not_Found_Test()
        {
            using ServicesDISetup _setup = new ServicesDISetup();
            var parentService = _setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var context = _setup.ServiceProvider.GetService<AppDbContext>();

            var parentData = AddParentVMData();
            var updateData = UpdateParentData();
            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            var AddParent = await parentService.AddNewParent(parentData);

            var deleteParent = await parentService.DeleteParent(1);
            var result = await parentService.UpdateParent(1, updateData);
            
            Assert.That(result.ErrorMessages.Contains($"No parent for id : 1"));
        }

        [Test]
        public async Task DeleteParent_Test()
        {
            using ServicesDISetup _setup = new ServicesDISetup();
            var parentService = _setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var context = _setup.ServiceProvider.GetService<AppDbContext>();

            var parentData = AddParentVMData();
            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            var AddParent = await parentService.AddNewParent(parentData);

            var result = await parentService.DeleteParent(1);

            Assert.That(result.Data.Contains("Deleted"));
        }

        [Test]
        public async Task DeleteParent_No_Parent_For_Id_Test()
        {
            using ServicesDISetup _setup = new ServicesDISetup();
            var parentService = _setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var context = _setup.ServiceProvider.GetService<AppDbContext>();

            var result = await parentService.DeleteParent(1);

            Assert.That(result.ErrorMessages.Contains("No parent for id : 1"));
        }

        [Test]
        public async Task GetParentById_Test()
        {
            using ServicesDISetup _setup = new ServicesDISetup();
            var parentService = _setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var context = _setup.ServiceProvider.GetService<AppDbContext>();

            var parentData = AddParentVMData();
            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            var AddParent = await parentService.AddNewParent(parentData);

            var result = await parentService.GetParentById(1);

            Assert.AreEqual(parentData.EmailAddress,result.Data.ContactEmail);
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

        private UpdateParentVM UpdateParentData()
        {
            var data = new UpdateParentVM()
            {
                Title = "Mr",
                FirstName = "Okedu",
                LastName = "Ekuka",
                Sex = "Male",
                Occupation = "Engineer",
                ModeOfIdentification = "NIN",
                IdentificationNumber = "9788892718",
                PhoneNumber = "09087675645",
                EmailAddress = "EbukaBued@gmail.com",
                HomeAddress = "7 kunle lawal",
                OfficeAddress = "17h karimu kolawate",
                DocumentType = DocumentType.Logo
            };
            return data;
        }
    }
}
