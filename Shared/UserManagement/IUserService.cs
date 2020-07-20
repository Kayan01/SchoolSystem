using Shared.ViewModels;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.UserManagement
{
    public interface IUserService
    {
        Task<IEnumerable<UserVM>> GetUsers(string claim, string email);
    }
}
