using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
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
using Microsoft.Extensions.Logging;
using Shared.Enums;
using static Shared.Utils.CoreConstants;
using Microsoft.Extensions.Configuration;
using Shared.DataAccess.Repository;
using Auth.Core.Context;
using Microsoft.EntityFrameworkCore;

namespace Auth.Core.Services
{
    public class AuthUserManagementService : IAuthUserManagement
    {
        private readonly UserManager<User> _userManager;
        private readonly IDataProtector _protector;
        private readonly IPublishService _publishService;
        private readonly IConfiguration _config;
        private AppDbContext _context;

        public AuthUserManagementService(
            UserManager<User> userManager,
            AppDbContext context,
            IDataProtectionProvider provider,
            IPublishService publishService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration config,
        ILogger<AuthUserManagementService> logger)
        {
            _userManager = userManager;
            _protector = provider.CreateProtector("Auth");
            _publishService = publishService;
            _config = config;
            _context = context;
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
        public async Task<ResultModel<bool>> SendRegistrationEmail(User user, string subdomain, string emailTitle = "Confirm your email")
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);


            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var sub = string.IsNullOrWhiteSpace(subdomain) ? "" : subdomain + ".";
            var clientURL = $"https://{sub}{_config["ClientURL"]}";


            if (!Uri.TryCreate(clientURL, UriKind.Absolute, out Uri uri))
            {
                return new ResultModel<bool>("School domains name is invalid");
            }

            var callbackUrl = $"{uri}#/email-verified?userId={user.Id}&code={code}";

            CreateEmailModel emailModel;

            if (user.UserType == UserType.SchoolAdmin)
            {
                emailModel = new CreateEmailModel(EmailTemplateType.NewSchool, new Dictionary<string, string>
                {
                    {"link", callbackUrl},
                }, user);
            }
            else if (user.UserType == UserType.SchoolGroupManager)
            {
                emailModel = new CreateEmailModel(EmailTemplateType.NewManager, new Dictionary<string, string>
                {
                    {"link", callbackUrl},
                }, user);
            }
            else
            {
                emailModel = new CreateEmailModel(EmailTemplateType.NewUser, new Dictionary<string, string>
                {
                    {"link", callbackUrl},
                    {"FullName", user.FullName },
                    {"Username", user.UserName }
                }, user);
            }

            await _publishService.PublishMessage(Topics.Notification, BusMessageTypes.NOTIFICATION, new CreateNotificationModel
            {
                Emails = new List<CreateEmailModel>
                {
                    emailModel
                },
                Notifications = new List<InAppNotificationModel>
                {
                    new InAppNotificationModel("New registration email was sent", EntityType.User, user.Id, new[] { user.Id }.ToList())
                }
            });

            return new ResultModel<bool>(true, "Success");

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



            //update user 
            if (user.IsFirstTimeLogin)
            {
                user.IsFirstTimeLogin = false;
            }
            await _userManager.UpdateAsync(user);

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
           
            return new ResultModel<(User user, string code)>((user, code), "Success");
        }

        private async Task SendPasswordResetEmail(User user, string code)
        {
            var link = $"https://{_config["ClientURL"]}/#/reset-password?code={code}";

            await _publishService.PublishMessage(Topics.Notification, BusMessageTypes.NOTIFICATION, new CreateNotificationModel
            {
                Emails = new List<CreateEmailModel>
                {
                    new CreateEmailModel(EmailTemplateType.PasswordReset, new Dictionary<string, string>{ { "link", link},  {"FullName", user.FullName }}, user)
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
                    new CreateEmailModel(EmailTemplateType.SuccessPasswordReset, new Dictionary<string, string>{ }, user)
                },
                Notifications = new List<InAppNotificationModel>
                {
                    new InAppNotificationModel("Password reset was successful", EntityType.User, null, new[] { user.Id }.ToList())
                }
            });
        }

        public async Task EnableUsersAsync(IEnumerable<long> ids)
        {
           var users = await _context.Users.Where(x => ids.Contains(x.Id)).ToListAsync();

            foreach (var user in users)
            {
                user.UserStatus = UserStatus.Activated;
                _context.Update(user);
            }

           await _context.SaveChangesAsync();
        }

        public async Task DisableUsersAsync(IEnumerable<long> ids)
        {
            var users = await _context.Users.Where(x => ids.Contains(x.Id)).ToListAsync();

            foreach (var user in users)
            {
                user.UserStatus = UserStatus.Deactivated;
                _context.Update(user);
            }

            await _context.SaveChangesAsync();
        }
    }
}