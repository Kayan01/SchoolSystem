using Auth.Core.Test.Services.Setup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Auth.Core.Services.Interfaces;
using Auth.Core.Models;
using Auth.Core.Context;
using Auth.Core.ViewModels.SchoolClass;
using Auth.Core.Services.Interfaces.Class;

namespace Auth.Core.Test.Services.ClassTest
{
    [TestFixture]
    class SchoolClassService_Test
    {
        [SetUp]
        public void Setup()
        { 
        }

        [Test]
        public async Task AddClass_Test()
        {
            using ServicesDISetup _setup = new ServicesDISetup();
            var ClassService = _setup.ServiceProvider.GetService<ISchoolClassService>();
            var _sectionService = _setup.ServiceProvider.GetService<ISectionService>();
            var _classArmService = _setup.ServiceProvider.GetService<IClassArmService>();
            var context = _setup.ServiceProvider.GetService<AppDbContext>();

            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();
            
            var AddSection = await _sectionService.AddSection(SectionModel);
            var AddClassArm = await _classArmService.AddClassArm(ClassArmModel);

            ClassModel.SectionId = AddSection.Data.Id;
            List<long> ListOfClassArms = new List<long>()
            {
                AddClassArm.Data.Id
            };
            ClassModel.ClassArmIds = ListOfClassArms;
            //Act
            var AddClass = await ClassService.AddClass(ClassModel);
            //Assert

            Assert.That(AddClass.ErrorMessages.Count == 0);
            Assert.That(AddClass.Data.Contains("Processed Successfully"));
        }

        private School AddSchoolData()
        {
            var school = new School()
            {
                Id = 4,
                DomainName = "Test",
                Name = "Test School"
            };

            return school;
        }
        private ClassSectionVM AddClassSectionData()
        {
            var model = new ClassSectionVM()
            {
                Name = "Testing",
            };

            return model;
        }
        private AddClassArm AddClassArmData()
        {
            var data = new AddClassArm()
            {
                Name = "YellowLabel",
                Status = true
            };
            return data;
        }
        private AddClassVM AddClassData()
        {
            var data = new AddClassVM()
            {
                Name = "Class1",
                Status = true,
                IsTerminalClass = false,
                Sequence = 3
            };
            return data;
        }
    }
}
