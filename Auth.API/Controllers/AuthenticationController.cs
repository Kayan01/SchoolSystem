﻿using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenIddict.Abstractions;
using OpenIddict.Mvc.Internal;
using OpenIddict.Server;
using Shared.AspNetCore;
using Shared.Entities;
using Shared.Timing;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.API.ViewModel;
using Auth.Core.Services.Interfaces;
using Shared.ViewModels.Enums;
using Auth.Core.ViewModels;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Shared.Enums;

namespace Auth.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    public class AuthenticationController : BaseController
    {
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthUserManagement _authUserService;

        public AuthenticationController(IOptions<IdentityOptions> identityOptions,
                                        SignInManager<User> signInManager,
                                        IAuthUserManagement authUserService,
                                        UserManager<User> userManager)
        {
            _identityOptions = identityOptions;
            _signInManager = signInManager;
            _userManager = userManager;
            _authUserService = authUserService;
        }

        [AllowAnonymous, HttpPost]
        public async Task<IActionResult> Token([ModelBinder(BinderType = typeof(OpenIddictMvcBinder))] OpenIdConnectRequest model)
        {
            try
            {
                if (model.IsPasswordGrantType())
                {
                    var applicationUser = await _userManager.FindByNameAsync(model.Username);

                    if (applicationUser is null)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "Login or password is incorrect."
                        });
                    }

                    // Ensure the user is allowed to sign in.
                    if (!await _signInManager.CanSignInAsync(applicationUser) || applicationUser.IsDeleted)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.AccessDenied,
                            ErrorDescription = "You are not allowed to sign in."
                        });
                    }

                    // Reject the token request if two-factor authentication has been enabled by the user.
                    if (_userManager.SupportsUserTwoFactor && await _userManager.GetTwoFactorEnabledAsync(applicationUser))
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.AccessDenied,
                            ErrorDescription = "You are not allowed to sign in."
                        });
                    }

                    // Ensure the user is not already locked out.
                    if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(applicationUser))
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.AccessDenied,
                            ErrorDescription = "Your profile is temporary locked."
                        });
                    }

                    // Ensure the password is valid.
                    if (!await _userManager.CheckPasswordAsync(applicationUser, model.Password))
                    {
                        if (_userManager.SupportsUserLockout)
                        {
                            await _userManager.AccessFailedAsync(applicationUser);
                        }

                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "Login or password is incorrect."
                        });
                    }



                    //check if user is logging in for the first time
                    if (applicationUser.IsFirstTimeLogin)
                    {
                        return BadRequest(new 
                        {
                            Error = "first_time_login",
                            ErrorDescription = "First time login. The user should change his password.",
                            PasswordResetCode = (await _authUserService.GetPasswordRestCode(applicationUser.Email)).Data.code
                        });
                    }

                    //check if school is disabled
                    if (applicationUser.UserStatus == UserStatus.Deactivated)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.AccessDenied,
                            ErrorDescription = "You have been disabled. Please contact admin."
                        });
                    }

                    if (_userManager.SupportsUserLockout)
                    {
                        await _userManager.ResetAccessFailedCountAsync(applicationUser);
                    }

                    applicationUser.LastLoginDate = Clock.Now;
                    await _userManager.UpdateAsync(applicationUser);

                    var ticket = await CreateTicketAsync(model, applicationUser);
                    return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
                }

                // Create a new authentication ticket.
                //var ticket = await CreateTicketAsync(model, applicationUser, isAdUser: isAdUser);
                //return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);

                else if (model.IsRefreshTokenGrantType())
                {
                    // Retrieve the claims principal stored in the refresh token.
                    var info = await HttpContext.AuthenticateAsync(
                        OpenIddictServerDefaults.AuthenticationScheme);

                    // Retrieve the user profile corresponding to the refresh token.
                    // Note: if you want to automatically invalidate the refresh token
                    // when the user password/roles change, use the following line instead:
                    // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
                    var user = await _userManager.GetUserAsync(info.Principal);
                    if (user == null)
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "The refresh token is no longer valid."
                        });
                    }

                    // Ensure the user is still allowed to sign in.
                    if (!await _signInManager.CanSignInAsync(user))
                    {
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = "The user is no longer allowed to sign in."
                        });
                    }

                    // Create a new authentication ticket, but reuse the properties stored
                    // in the refresh token, including the scopes originally granted.
                    var ticket = await CreateTicketAsync(model, user, info.Properties);
                    return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
                }

                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    ErrorDescription = "The specified grant type is not supported."
                });
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest oidcRequest, User user,
    AuthenticationProperties properties = null)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;
            AddUserInformationClaims(user, identity);

            var ticket = new AuthenticationTicket(principal, properties, OpenIddictServerDefaults.AuthenticationScheme);

            if (!oidcRequest.IsRefreshTokenGrantType())
            {
                // Set the list of scopes granted to the client application.
                // Note: the offline_access scope must be granted
                // to allow OpenIddict to return a refresh token.
                ticket.SetScopes(new[]
                {
                    OpenIdConnectConstants.Scopes.OpenId,
                    OpenIdConnectConstants.Scopes.Email,
                    OpenIdConnectConstants.Scopes.Profile,
                    OpenIdConnectConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.Roles,

                }.Intersect(oidcRequest.GetScopes()));
            }

            ticket.SetResources("resource_server");

            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.
            var destinations = new List<string>
            {
                OpenIdConnectConstants.Destinations.AccessToken
            };

            foreach (var claim in ticket.Principal.Claims)
            {
                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
                {
                    continue;
                }

                // Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
                // The other claims will only be added to the access_token, which is encrypted when using the default format.
                if ((claim.Type == OpenIdConnectConstants.Claims.Name && ticket.HasScope(OpenIdConnectConstants.Scopes.Profile)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Email && ticket.HasScope(OpenIdConnectConstants.Scopes.Email)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Role && ticket.HasScope(OpenIddictConstants.Claims.Roles)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Audience && ticket.HasScope(OpenIddictConstants.Claims.Audience))

                    )
                {
                    destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);
                }

                claim.SetDestinations(destinations);
            }

            //var name = new Claim(OpenIdConnectConstants.Claims.GivenName, user.FullName ?? "[NA]");
            //name.SetDestinations(OpenIdConnectConstants.Destinations.AccessToken);
            //identity.AddClaim(name);

            return ticket;
        }

        private void AddUserInformationClaims(User user, ClaimsIdentity identity)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            if (!string.IsNullOrWhiteSpace(user.FirstName))
            {
                identity.AddClaim(new Claim(JwtClaimTypes.GivenName, user.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(user.LastName))
            {
                identity.AddClaim(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            }

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                identity.AddClaim(new Claim(JwtClaimTypes.Email, user.Email));
            }
            identity.AddClaim(new Claim("last_login_time", user.LastLoginDate.Value.ToString("f")));
        }

        [AllowAnonymous, HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }

            return Ok("Done !!!");
        }


        [HttpPost()]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [AllowAnonymous]
        public async Task<IActionResult> RequestPasswordReset([FromForm]string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                    return ApiResponse<string>(errors: "A valid email is required");

                var result = await _authUserService.RequestPasswordReset(userName);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<string>(message: result.Message, codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception e)
            {
                return  HandleError(e);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordReset([FromBody]PasswordResetModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Token))
                    return ApiResponse<string>(errors: "Token is required");
                if (string.IsNullOrEmpty(model.NewPassword))
                    return ApiResponse<string>(errors: "Password is required");

                var result = await _authUserService.PassworReset(model);

                if (result.HasError)
                    return ApiResponse<bool>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<bool>(message: result.Message, codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {

            try
            {

                if (string.IsNullOrEmpty(userId))
                    return ApiResponse<string>(errors: "UserId is required");
                if (string.IsNullOrEmpty(code))
                    return ApiResponse<string>(errors: "code is required");



                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<string>(errors: $"Unable to load user with ID '{userId}'.");
                }

                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (!result.Succeeded)
                {
                    return ApiResponse<string>(errors: result.Errors.Select(x=> x.Description).ToArray());
                }

                var PasswordResetCode = (await _authUserService.GetPasswordRestCode(user.UserName)).Data.code;

                return ApiResponse<string>(message: "User confirmed", data : PasswordResetCode, codes: ApiResponseCodes.OK);
            }
            catch (Exception ex)
            {
                return  HandleError(ex);
            }
         
        }

    }
}