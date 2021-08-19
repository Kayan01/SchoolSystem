using Auth.Core.Context;
using Auth.Core.Models;
using Auth.Core.Services.Interfaces.Class;
using Auth.Core.Test.Services.Setup;
using Auth.Core.ViewModels.SchoolClass;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shared.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Auth.Core.Test.Services.SchoolTests
{
    [TestFixture]
    public class SectionService_Test
    {        
        readonly string expectedOutput = "Section could not be found";

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public async Task AddSectionTest()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _sectionServices = _setup.ServiceProvider.GetService<ISectionService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var school = new School()
                {
                    Id = 4,
                    DomainName = "Test",
                    Name = "Test School"
                };
                context.Schools.Add(school);
                await context.SaveChangesAsync();

                var model = new ClassSectionVM()
                {
                    Name = "Testing",
                };

                var result = await _sectionServices.AddSection(model);

                Assert.That(!result.HasError);
                Assert.AreEqual(model, result.Data);
            }

        }

        [Test]
        public async Task UpdateSectionTest_Valid_Id()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _SectionService = (ISectionService)_setup.ServiceProvider.GetService(typeof(ISectionService));
                var context = (AppDbContext)_setup.ServiceProvider.GetService(typeof(AppDbContext));

                var school = new School()
                {
                    Id = 4,
                    DomainName = "Test",
                    Name = "Test School"
                };
                context.SchoolSections.Add(new SchoolSection()
                {
                    Id = 3,
                    Name = "Testestomus",
                    School = school
                });
                await context.SaveChangesAsync();

                var model = new ClassSectionUpdateVM()
                {
                    Name = "Testing",
                };
                var result = await _SectionService.UpdateSection(3, model);
                Assert.That(!result.HasError);
                Assert.AreEqual(model.Name, result.Data.Name);
            }
        }

        [Test]
        public async Task UpdateSectionTest_Id_Not_Found()
        {
            using(ServicesDISetup _setup = new ServicesDISetup())
            {
                var _SectionService = (ISectionService)_setup.ServiceProvider.GetService(typeof(ISectionService));
                var context = (AppDbContext)_setup.ServiceProvider.GetService(typeof(AppDbContext));
                
                var model = new ClassSectionUpdateVM()
                {
                    Name = "Testing",
                };
                var result = await _SectionService.UpdateSection(4, model);

                Assert.That(result.ErrorMessages.Contains(expectedOutput));
            }
            
        }

        [Test]
        public async Task GetAllSections()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _sectionService = _setup.ServiceProvider.GetService<ISectionService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();


                var school = new School()
                {
                    Id = 4,
                    DomainName = "Test",
                    Name = "Test School"
                };
                context.Schools.Add(school);
                await context.SaveChangesAsync();

                var model = new ClassSectionVM()
                {
                    Name = "Testing",
                };

                var AddSection = await _sectionService.AddSection(model);

                var queryModel = new QueryModel()
                {
                    PageIndex =1,
                    PageSize = 3
                };

                var result = await _sectionService.GetAllSections(queryModel);

                Assert.That(!result.HasError);
                Assert.AreEqual(1,result.Data.TotalItemCount);
            }
        }

        [Test]
        public async Task Get_Sections_By_Id()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _sectionService = _setup.ServiceProvider.GetService<ISectionService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var school = new School()
                {
                    Id = 4,
                    DomainName = "Test",
                    Name = "Test School"
                };
                context.Schools.Add(school);
                await context.SaveChangesAsync();

                var model = new ClassSectionVM()
                {
                    Name = "Testing",
                };

                var AddSection = await _sectionService.AddSection(model);

                var result = await _sectionService.GetSectionById(3);

                Assert.That(!result.HasError);
                Assert.AreEqual("Testing", result.Data.Name);
            }
        }

        [Test]
        public async Task Get_Sections_Id_NotFound()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _sectionService = _setup.ServiceProvider.GetService<ISectionService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var result = await _sectionService.GetSectionById(3);

                Assert.That(result.ErrorMessages.Contains(expectedOutput));
            }
        }

        [Test]
        public async Task DeleteSection_Test()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _sectionService = _setup.ServiceProvider.GetService<ISectionService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var school = new School()
                {
                    Id = 4,
                    DomainName = "Test",
                    Name = "Test School"
                };
                context.Schools.Add(school);
                await context.SaveChangesAsync();

                var model = new ClassSectionVM()
                {
                    Name = "Testing",
                };

                var AddSection = await _sectionService.AddSection(model);

                Assert.That(!(AddSection.ErrorMessages.Contains("Operation Failed")));

                var result =await _sectionService.DeleteSection(3);

                Assert.That(result.Data == true);
            }
        }
        
        [Test]
        public async Task DeleteSection_Id_Not_Found_Test()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _sectionService = _setup.ServiceProvider.GetService<ISectionService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var result = await _sectionService.DeleteSection(1);

                Assert.That(result.Data == false);

            }
        }
    }
}
