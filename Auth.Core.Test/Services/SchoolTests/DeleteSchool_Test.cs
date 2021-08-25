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

namespace Auth.Core.Test.Services.SchoolTests
{
    [TestFixture]
    class DeleteSchool_Test
    {
        [Test]
        public async Task DeleteSchol()
        {
            using(ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();


                context.Schools.Add(new School()
                {
                    DomainName = "Test",
                    Name = "Test School"
                });
                await context.SaveChangesAsync();


                var result =await _schoolService.DeleteSchool(4);

                Assert.That(result.Data == true);
            }
        }
        [Test]
        public async Task DeleteSchool_School_Does_Not_Exist()
        {
            using(ServicesDISetup _setup = new ServicesDISetup())
            {
                var _schoolService = _setup.ServiceProvider.GetService<ISchoolService>();

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

                var result = await schoolService.GetTotalSchoolsCount();

                Assert.That(result.Data > 0);
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
