using Auth.Core.Test.Services.Setup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Auth.Core.Services.Interfaces;
using Auth.Core.Context;
using Auth.Core.ViewModels;
using Shared.Entities;
using Shared.Enums;
using Microsoft.AspNetCore.Http;
using Shared.ViewModels;

namespace Auth.Core.Test.Services.AdminTest
{
    [TestFixture]
    class AddAdmin_Test
    {
        [Test]
        public async Task AddAdminTest()
        {
            using(ServicesDISetup _setup = new ServicesDISetup())
            {
                var _AdminService = _setup.ServiceProvider.GetService<IAdminService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var AddAdminData = AddAminData();

                var result = await _AdminService.AddAdmin(AddAdminData);

                Assert.That(result.ErrorMessages.Count == 0);
                Assert.AreEqual(AddAdminData.Email, result.Data.Email);

            }
        }
        [Test]
        public async Task DeleteAdminTest()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _AdminService = _setup.ServiceProvider.GetService<IAdminService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var AddAdminData = AddAminData();

                var AdminAdded = await _AdminService.AddAdmin(AddAdminData);
                var result = await _AdminService.DeleteAdmin(AdminAdded.Data.Id);

                Assert.That(result.Data == true);


            }
        }
        [Test]
        public async Task DeleteAdmin_NotFound_Test()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _AdminService = _setup.ServiceProvider.GetService<IAdminService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var result = await _AdminService.DeleteAdmin(6);

                Assert.That(result.Data == false);
                Assert.That(result.ErrorMessages.Contains("Admin not found"));

            }
        }
       
        [Test]
        public async Task GetAdminById_Test()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _AdminService = _setup.ServiceProvider.GetService<IAdminService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var AddAdminData = AddAminData();

                var AdminAdded = await _AdminService.AddAdmin(AddAdminData);
                var result = await _AdminService.GetAdminById(AdminAdded.Data.Id);

                Assert.That(result.ErrorMessages.Count == 0);
                Assert.That(result.Data.FirstName == AddAdminData.FirstName);
            }
        }

        [Test]
        public async Task GetAllAdmin_Test()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _AdminService = _setup.ServiceProvider.GetService<IAdminService>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var AddAdminData = AddAminData();

                var AdminAdded = await _AdminService.AddAdmin(AddAdminData);
                var queryModel = new QueryModel()
                {
                    PageIndex = 1,
                    PageSize = 1
                };

                var result = await _AdminService.GetAllAdmin(queryModel);
                Assert.That(result.ErrorMessages.Count == 0);
                Assert.That(result.Data.TotalItemCount == 2);
            }
        }

        [Test]
        public async Task UpdateAdmin_Test()
        {
            using(ServicesDISetup _setup = new ServicesDISetup())
            {
                var _AdminService = _setup.ServiceProvider.GetService<IAdminService>();
                var _content = _setup.ServiceProvider.GetService<AppDbContext>();

                var AddAdminData = AddAminData();
                var AdminAdded = await _AdminService.AddAdmin(AddAdminData);

                var updateModel = new UpdateAdminVM()
                {
                    UserId = 2,
                    Email = "bubu@gmail.com",
                    FirstName = "Andrey",
                    LastName = "Gab",
                    PhoneNumber = "08067564534",
                    UserName = "bubu@gmail.com"
                };

                var result = await _AdminService.UpdateAdmin(updateModel);

                Assert.That(result.ErrorMessages.Count == 0);
                Assert.AreEqual(updateModel.Email,result.Data.Email);
            }
        }

        [Test]
        public async Task UpdateAdmin_Admin_Not_FoUnd_Test()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _AdminService = _setup.ServiceProvider.GetService<IAdminService>();
                var _content = _setup.ServiceProvider.GetService<AppDbContext>();

                var updateModel = new UpdateAdminVM()
                {
                    UserId = 2,
                    Email = "bubu@gmail.com",
                    FirstName = "Andrey",
                    LastName = "Gab",
                    PhoneNumber = "08067564534",
                    UserName = "bubu@gmail.com"
                };

                var result = await _AdminService.UpdateAdmin(updateModel);

                Assert.That(result.ErrorMessages.Contains("No admin found"));
            }
        }


        private AddAdminVM AddAminData()
        {

            var docType = DocumentType.Logo;
            List<DocumentType> docTypeList = new List<DocumentType>() { docType };
            var newAdmin = new AddAdminVM()
            {
                Email = "buedgabby@gmail.com",
                FirstName = "Ade",
                LastName = "Ola",
                UserName = "buedgabby@gmail.com",
                PhoneNumber = "09089765645",
                DocumentTypes = docTypeList,

            };
            return newAdmin;
        }
    }
}
