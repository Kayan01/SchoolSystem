using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using OpenIddict.Validation;
using Auth.Core.Context;
using Shared.Configuration;
using Shared.DataAccess.EfCore.Context;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Auth.API
{
    public partial class Startup
    {
        private async Task InitializeIdentityDbAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            using var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

            if (await manager.FindByClientIdAsync("APP.base", cancellationToken) == null)
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = "APP.base",
                    ClientSecret = "178e196b-04b5-40ff-b235-7ac541eed1c9",
                    DisplayName = "APP base Api client",
                    Type = OpenIddictConstants.ClientTypes.Hybrid,
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles
                    }
                };

                await manager.CreateAsync(descriptor, cancellationToken);
            }
        }

        public void AddIdentityProvider(IServiceCollection services)
        {
            var authSettings = new OpenIddictServerConfig();

            Configuration.Bind(nameof(OpenIddictServerConfig), authSettings);
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings.SecretKey));

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequiredLength = 6;

                //allow unicode characters for regnumbers
                var allowedUserNameCharacters = options.User.AllowedUserNameCharacters + @"\/,.";
                options.User.AllowedUserNameCharacters = allowedUserNameCharacters;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 3;
            });

            var x509Certificate = new X509Certificate2(Path.Combine(
                          HostingEnvironment.ContentRootPath, "auth.pfx")
                      , "idsrv3test", X509KeyStorageFlags.MachineKeySet);

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<AppDbContext>();
                })
                .AddServer(options =>
                {
                    options.RegisterScopes(OpenIdConnectConstants.Scopes.Email,
                        OpenIdConnectConstants.Scopes.Profile,
                        OpenIdConnectConstants.Scopes.Address,
                        OpenIdConnectConstants.Scopes.Phone,
                        OpenIddictConstants.Scopes.Roles,
                        OpenIdConnectConstants.Scopes.OfflineAccess,
                        OpenIdConnectConstants.Scopes.OpenId
                    );

                    if (!authSettings.RequireHttps)
                        options.DisableHttpsRequirement();

                    options.EnableTokenEndpoint("/api/v1/authentication/token")
                        .AllowRefreshTokenFlow()
                        .AcceptAnonymousClients()
                        .AllowPasswordFlow()
                        .SetAccessTokenLifetime(TimeSpan.FromDays(1))
                        .SetIdentityTokenLifetime(TimeSpan.FromDays(1))
                        .SetRefreshTokenLifetime(TimeSpan.FromMinutes(120))
                        .AddSigningCertificate(x509Certificate)
                        .UseJsonWebTokens();
                });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = OpenIddictValidationDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = OpenIddictValidationDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = options.Authority = authSettings.Authority;
                options.RequireHttpsMetadata = authSettings.RequireHttps;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = OpenIdConnectConstants.Claims.Name,
                    RoleClaimType = OpenIdConnectConstants.Claims.Role,
                    IssuerSigningKey = signingKey,
                    ValidateAudience = authSettings.ValidateAudience,
                    ValidateIssuer = authSettings.ValidateIssuer,
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        public void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 3;
            })
           .AddEntityFrameworkStores<AppDbContext>()
           .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(24);
            });
        }


    }
}
