using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AssessmentSvc.Core.Context;
using Shared.Collections;
using Shared.Infrastructure.HealthChecks;
using Shared.Utils;

namespace AssessmentSvc.API
{
    public partial class Startup
    {
        private IWebHostEnvironment HostingEnvironment { get; set; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = webHostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy("MyCorsPolicy",
                    builder => builder.WithOrigins(Configuration["AllowedCorsOrigin"].Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray())
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins("https://*.myschooltrack.com")
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .Build()
   )
                );
            services.AddSwagger("Assessment Service");
            services.AddControllers().ConfigureApiBehaviorOptions(c => c.SuppressModelStateInvalidFilter = true);

            AddEntityFrameworkDbContext(services);
            AddIdentityProvider(services);
            ConfigureDIService(services);
            services.RegisterDinkToPdfTool();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("MyCorsPolicy");

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCustomSwaggerApi("Assessment Service");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                .RequireAuthorization();
            });


            app.UseCustomHealthChecksAPI();

        }

        public void AddEntityFrameworkDbContext(IServiceCollection services)
        {
            string dbConnStr = Configuration.GetConnectionString("Default");

            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseSqlServer(dbConnStr,
                 b => b.MigrationsAssembly(typeof(AppDbContext).FullName));
            });
        }
    }
}
