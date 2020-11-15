using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Shared.Entities;
using Shared.PubSub;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Utils.CoreConstants;

namespace Auth.Core.Services
{
    public class AuthUserManagementService : IAuthUserManagement
    {
        private readonly UserManager<User> _userManager;
        private readonly IDataProtector _protector;
        private readonly IPublishService _publishService;

        public AuthUserManagementService(
            UserManager<User> userManager,
            IDataProtectionProvider provider,
            IPublishService publishService)
        {
            _userManager = userManager;
            _protector = provider.CreateProtector("Auth");
            _publishService = publishService;
        }

        public async Task<long?> AddUserAsync(AuthUserModel model)
        {
            var user = new User { Email = model.Email, UserName = model.Email, FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.PhoneNumber };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return user.Id;
            }

            return null;
        }

        public async Task<bool> DeleteUserAsync(long id)
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

        public async Task<bool> UpdateUserAsync(long id, AuthUserModel model)
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

        public async Task<ResultModel<string>> RequestPasswordReset(string email)
        {
            var result = await GetPasswordRestCode(email);

            if (result.HasError)
                return new ResultModel<string>(result.ErrorMessages);

            await SendPasswordResetEmail(result.Data.user, result.Data.code);

            return new ResultModel<string>(email, "Success");
        }

        public async Task<ResultModel<bool>> PassworReset(PasswordResetModel model)
        {            
            var passwordResetModelString = "";
            try
            {
                passwordResetModelString = _protector.Unprotect(model.Token);
            }
            catch (Exception e)
            {
                return new ResultModel<bool>("Invalid Token");
            }

            var passwordResetModel = JsonConvert.DeserializeObject<PasswordResetQueryModel>(passwordResetModelString);
            //passwordResetModel.Token = WebUtility.UrlDecode(passwordResetModel.Token);
            var user = await _userManager.FindByEmailAsync(passwordResetModel.Email);
            if (user == null)
                return new ResultModel<bool>($"User with {passwordResetModel?.Email} does not exist");

            if (!user.EmailConfirmed)
                user.EmailConfirmed = true;

            //Update Password
            var res = await _userManager.ResetPasswordAsync(user, passwordResetModel.Token, model.NewPassword);
            if (!res.Succeeded)
                return new ResultModel<bool>("Failed to reset password");

            await SendSuccessfulPasswordResetMessage(user);

            return new ResultModel<bool>(true, "Success");
        }

        public async Task<ResultModel<(User user, string code)>> GetPasswordRestCode(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ResultModel<(User user, string code)>($"User with {email} does not exist");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var tokenQueryModel = new PasswordResetQueryModel { Email = user.Email, Token = code };
            var tokenQueryModelString = JsonConvert.SerializeObject(tokenQueryModel);
            code = _protector.Protect(tokenQueryModelString);
            //code = tokenQueryModelString;
            //code = WebUtility.UrlEncode(code);
            return new ResultModel<(User user, string code)>((user, code), "Success");
        }

        private async Task SendPasswordResetEmail(User user, string code)
        {
            await _publishService.PublishMessage(Topics.Notification, BusMessageTypes.NOTIFICATION, new CreateNotificationModel
            {
                Emails = new List<CreateEmailModel>
                {
                    new CreateEmailModel(EmailTemplateType.PasswordReset, new StringDictionary{ { "Code", code} }, user)
                },
                Notifications = new List<InAppNotificationModel>
                {
                    new InAppNotificationModel("Password reset email was sent", EntityType.User, user.Id, new[] { user.Id }.ToList())
                }
            });
        }

        private async Task SendSuccessfulPasswordResetMessage(User user)
        {
            await _publishService.PublishMessage(Topics.Notification, BusMessageTypes.NOTIFICATION, new CreateNotificationModel
            {
                Emails = new List<CreateEmailModel>
                {
                    new CreateEmailModel(EmailTemplateType.SuccessPasswordReset, new StringDictionary{ }, user)
                },
                Notifications = new List<InAppNotificationModel>
                {
                    new InAppNotificationModel("Password reset was successful", EntityType.User, null, new[] { user.Id }.ToList())
                }
            });
        }
    }
}