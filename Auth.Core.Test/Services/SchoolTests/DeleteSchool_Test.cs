using Auth.Core.Test.Services.Setup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Auth.Core.Services.Interfaces;
using Auth.Core.Context;
using Auth.Core.Models;
using NUnit.Framework;
using Shared.ViewModels;
using System.Linq;

namespace Auth.Core.Test.Services.SchoolTests
{
    [TestFixture]
    class DeleteSchool_Test
    {
        [Test]
        public async Task DeleteSchool_School_Exist_Test()
        {
            using(ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                //Arrange
                var newSchool = new School()
                {
                    DomainName = "Test",
                    Name = "Test School"
                };
                context.Schools.Add(newSchool);
                await context.SaveChangesAsync();
                
                //Act
                var result =await _schoolService.DeleteSchool(newSchool.Id);
                //Assert
                Assert.That(result.Data == true);
                // Validate School Has been deleted
                var schoolExistCheck = await _schoolService.GetSchoolById(newSchool.Id);
                Assert.That(schoolExistCheck.ErrorMessages.Contains("No school found"));
            }
        }
        [Test]
        public async Task DeleteSchool_School_Does_Not_Exist()
        {
            using(ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();

                //Passing in a wrong Id to check that school does not exist.
                var result = await _schoolService.DeleteSchool(5);

                Assert.That(result.ErrorMessages.Contains("School does not exist"));
                Assert.That(result.Data == false);
            }

        }

        [Test]
        public async Task Get_Total_School_Count_Test()
        {
            using(ServicesDISetup _setup = new ServicesDISetup())
            {
                var schoolService = _setup.ServiceProvider.GetService<ISchoolService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                //Delete all seeded Data
                var allSchools = context.Schools.ToList();
                foreach (var item in allSchools)
                {
                    var deleteSchool = schoolService.DeleteSchool(item.Id);
                }
                var checkSchoolCount = await schoolService.GetTotalSchoolsCount();
                Assert.That(checkSchoolCount.Data == 0);


                //Arrange
                var newSchool = new School()
                {
                    DomainName = "Test",
                    Name = "Test School"
                };
                context.Schools.Add(newSchool);
                await context.SaveChangesAsync();


                //Act
                var result = await schoolService.GetTotalSchoolsCount();
                //Assert
                Assert.That(result.Data == 1);
                Assert.That(result.ErrorMessages.Count == 0);
            }
        }

        [Test]
        public async Task Get_School_ExcelSheet_Test()
        {
              using(ServicesDISetup _setup = new ServicesDISetup())
            {
                var schoolService = _setup.ServiceProvider.GetService<ISchoolService>();

                var result = await schoolService.GetSchoolExcelSheet();

                Assert.That(result.ErrorMessages.Count == 0);
            }
        }
    }
}
