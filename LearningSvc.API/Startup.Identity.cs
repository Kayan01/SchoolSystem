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

namespace LearningSvc.API
{
    public partial class Startup
    {
        public void AddIdentityProvider(IServiceCollection services)
        {
            var authSettings = new OpenIddictServerConfig();

            Configuration.Bind(nameof(OpenIddictServerConfig), authSettings);
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings.SecretKey));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = OpenIddictValidationDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = OpenIddictValidationDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = authSettings.Authority;
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

    }
}
