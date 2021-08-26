using Auth.Core.Services.Interfaces;
using Auth.Core.Test.Services.Setup;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Auth.Core.Context;
using Auth.Core.ViewModels;
using Shared.Enums;
using System;
using System.Linq;
using Shared.Entities;
using System.Collections.Generic;

namespace Auth.Core.Test.Services.Users
{
    [TestFixture]
    class AuthUserManagementServiceTest
    {
        [SetUp]
        public void SetUp()
        {
        }
    
        [Test]
        public async Task AddUserTest()
        {
            using(ServicesDISetup _setup = new ServicesDISetup())
            {
                var _AuthUserManagementService = _setup.ServiceProvider.GetService<IAuthUserManagement>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var newAuthUserModel = AuthUserModelData();

                var result = await _AuthUserManagementService.AddUserAsync(newAuthUserModel);
                
                Assert.That(result.HasValue);
                //Console.WriteLine(result.GetValueOrDefault());
            }
            
        }

        [Test]
        public async Task DeleteUserAsync()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _AuthUserManagementService = _setup.ServiceProvider.GetService<IAuthUserManagement>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var newAuthUserModel = AuthUserModelData();

                var addAdmin = await _AuthUserManagementService.AddUserAsync(newAuthUserModel);
                var Id = addAdmin.GetValueOrDefault();

                var result = await _AuthUserManagementService.DeleteUserAsync(Id);

                Assert.That(result == true);
            }
        }
        [Test]
        public async Task GetAllUser_Test()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _AuthUserManagementService = _setup.ServiceProvider.GetService<IAuthUserManagement>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var newAuthUser = UserData();
                context.Users.Add(newAuthUser);
                await context.SaveChangesAsync();

                var result =_AuthUserManagementService.GetAllAuthUsersAsync();
                List<User> data = new List<User>();
                foreach(var item in result)
                {
                    data.Add(item);
                }

                Assert.That(data.Count == 2);
                Assert.AreEqual(newAuthUser.FullName,data[1].FullName);

            }
        }
        [Test]
        public async Task UpdateUserAsyncTest()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _AuthUserManagementService = _setup.ServiceProvider.GetService<IAuthUserManagement>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var newAuthUserModel = AuthUserModelData();

                var addAdmin = await _AuthUserManagementService.AddUserAsync(newAuthUserModel);
                var Id = addAdmin.GetValueOrDefault();

                var UpdateData = UpdateUserModel();
                var result = await _AuthUserManagementService.UpdateUserAsync(Id, UpdateData);

                Assert.That(result == true);

            }
        }

        [Test]
        public async Task SendRegistrationEmailTest()
        {
            using(ServicesDISetup _setup = new ServicesDISetup())
            {
                var _AuthUserManagementService = _setup.ServiceProvider.GetService<IAuthUserManagement>();
                var context = _setup.ServiceProvider.GetService<AppDbContext>();

                var newAuthUserModels = AuthUserModelData();
                var addAmin = _AuthUserManagementService.AddUserAsync(newAuthUserModels);
                var getAdmin = _AuthUserManagementService.GetAllAuthUsersAsync();
                List<User> data = new List<User>();
                foreach (var item in getAdmin)
                {
                    data.Add(item);
                }

                var result = await _AuthUserManagementService.SendRegistrationEmail(data[1], "");

                Assert.That(result.ErrorMessages.Count == 0);
                Assert.That(result.Data == true);
            }
        }
        
        [Test]
        public async Task RequestPasswordReset()
        {
            using(ServicesDISetup _Setup = new ServicesDISetup())
            {
                var _AuthUserManagement = _Setup.ServiceProvider.GetService<IAuthUserManagement>();
                var context = _Setup.ServiceProvider.GetService<AppDbContext>();

                var newAuthUserModels = AuthUserModelData();
                var addAmin = _AuthUserManagement.AddUserAsync(newAuthUserModels);

                var result = await _AuthUserManagement.RequestPasswordReset(newAuthUserModels.Email);

                Assert.That(result.ErrorMessages.Count == 0);
                Assert.That(result.Message.Contains("Success"));
                Assert.That(result.Data.Contains(newAuthUserModels.Email));
            }
        }



        private AuthUserModel AuthUserModelData()
        {
            var newAuthUserModel = new AuthUserModel()
            {
                FirstName = "Ade",
                LastName = "Ola",
                Email = "AdeOla@gmsil.com",
                PhoneNumber = "09089787632",
                Password = "GabbySTeams1990",
                UserType = UserType.GlobalAdmin
            };

            return newAuthUserModel;
        }

        private AuthUserModel UpdateUserModel()
        {
            var newAuthUserModel = new AuthUserModel()
            {
                FirstName = "AdeUpdate",
                LastName = "OlaUpdate",
            };

            return newAuthUserModel;
        }

        private User UserData()
        {
            var user = new User()
            {
                FirstName = " Ade",
                Unit = "1",
                MiddleName = "Osas",
                LastName = "gab",
                IsDeleted = false,
                Email = "buedgabby@gmail.com",
                EmailConfirmed = false,
                UserName = "buedgabby@gmail.com",
                ConcurrencyStamp = "kjuhgthdgga77865",
                UserType = UserType.GlobalAdmin
            };

            return user;
        }
    }
}
