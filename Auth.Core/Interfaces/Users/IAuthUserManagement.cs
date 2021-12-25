using Auth.Core.ViewModels;
using Shared.Entities;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services.Interfaces
{
    public interface IAuthUserManagement
    {
        //adds user and returns id
        Task<long?> AddUserAsync(AuthUserModel model);
        Task<bool> UpdateUserAsync(long id, AuthUserModel model);
        Task<bool> DeleteUserAsync(long id);
        Task EnableUsersAsync(IEnumerable<long> id);
        Task DisableUsersAsync(IEnumerable<long> id);
        IQueryable<User> GetAllAuthUsersAsync();

        Task<ResultModel<bool>> SendRegistrationEmail(User user, string subdomain, string schoolName, string schoolEmail, string address, string phoneNumber, string EmailPassword, string emailTitle = "Confirm your email");
        Task<ResultModel<string>> RequestPasswordReset(string userName);
        Task<ResultModel<bool>> PassworReset(PasswordResetModel model);
        Task<ResultModel<(User user, string code)>> GetPasswordRestCode(string email);
    }
}