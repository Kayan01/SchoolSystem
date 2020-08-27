using Auth.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services
{
    public class AuthUserManagementService : IAuthUserManagement
    {
        private readonly UserManager<User> _userManager;
        public AuthUserManagementService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<int?> AddUserAsync(string firstName, string lastName, string email, string phoneNumber, string pwd)
        {
            var user = new User { Email = email, FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber };

           var result = await _userManager.CreateAsync(user, pwd);

            if (result.Succeeded)
            {
                return user.Id;
            }


            return null;
        }


        public async Task<bool> DeleteUserAsync(int id)
        {
            var usr = await _userManager.FindByIdAsync(id.ToString());

            var result = await _userManager.DeleteAsync(usr);

            if (result.Succeeded)
            {
                return true;
            }


            return false;
        }

        public async Task<bool> UpdateUserAsync(int id, string firstName, string lastName)
        {
            var usr = await _userManager.FindByIdAsync(id.ToString());

            usr.FirstName = firstName;
            usr.LastName = lastName;

            var result = await _userManager.UpdateAsync(usr);

            if (result.Succeeded)
            {
                return true;
            }


            return false;
        }
    }
}
