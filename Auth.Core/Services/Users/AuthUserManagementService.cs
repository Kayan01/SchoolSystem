using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<int?> AddUserAsync(AuthUserModel model)
        {
            var user = new User { Email = model.Email, UserName = model.Email, FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.PhoneNumber };

            var result = await _userManager.CreateAsync(user, model.Password);

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

        public IQueryable<User> GetAllAuthUsersAsync()
        {
            return _userManager.Users;
        }

        public async Task<bool> UpdateUserAsync(int id, AuthUserModel model)
        {
            var usr = await _userManager.FindByIdAsync(id.ToString());

            usr.FirstName = model.FirstName;
            usr.LastName = model.LastName;

            var result = await _userManager.UpdateAsync(usr);

            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }
    }
}