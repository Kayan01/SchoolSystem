using Microsoft.AspNetCore.Identity;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Shared.ViewModels;

namespace Shared.UserManagement
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserVM>> GetUsers(string claim, string email)
        {
            var sampleClaim = new Claim("CLAIM", claim);
            var userResults = await _userManager.GetUsersForClaimAsync(sampleClaim);
            IEnumerable<User> users = userResults;
            if (!string.IsNullOrEmpty(email))
                users = userResults.Where(x => x.Email.ToLower() == email.ToLower());

            return users.Select(x => (UserVM)x);
        }
    }
}
