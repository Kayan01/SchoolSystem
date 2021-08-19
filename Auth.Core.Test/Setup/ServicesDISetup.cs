using Auth.Core.Context;
using Auth.Core.Models;
using Auth.Core.Services;
using Auth.Core.Services.Class;
using Auth.Core.Services.Interfaces;
using Auth.Core.Services.Interfaces.Class;
using Auth.Core.Test.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shared.DataAccess.EfCore;
using Shared.DataAccess.EfCore.Context;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Entities;
using Shared.FileStorage;
using Shared.PubSub;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using static Shared.Utils.CoreConstants;

namespace Auth.Core.Test.Services.Setup
{
    public class ServicesDISetup : IDisposable
    {
        public ServiceCollection services { get; private set; }
        public ServiceProvider ServiceProvider { get; protected set; }
        //public ISchoolService _schoolService;

        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;

        protected readonly AppDbContext DbContext;

        public ServicesDISetup()
        {
            services = new ServiceCollection();

            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();

            services.AddTransient<DbContext, AppDbContext>();
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlite(_connection));
            
            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));

            // Add Identity using in memory database to create UserManager and RoleManager.
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
            }).AddEntityFrameworkStores<AppDbContext>()
           .AddDefaultTokenProviders();

            services.RegisterGenericRepos(typeof(AppDbContext));

            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider("/FileStore"));
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IDocumentService, DocumentService>();

            var moqPublish = new MockPublishService();
            var moqFileUpload = new MockFileUploadService();
            var moqHttpAccessor = new MockhttpContextAccessorclass();

            services.AddScoped<IPublishService>(_ => moqPublish.Mock.Object);
            services.AddScoped<IDocumentService>(_ => moqFileUpload.Mock.Object);
            services.AddScoped<IHttpContextAccessor>(_ => moqHttpAccessor.Mock.Object);

            services.AddScoped<IAuthUserManagement, AuthUserManagementService>();
            var builder = new ConfigurationBuilder()
                //.SetBasePath("path here") //<--You would need to set the path
                .AddJsonFile("appsettings.json"); //or what ever file you have the settings

            IConfiguration configuration = builder.Build();

            services.AddScoped<IConfiguration>(_ => configuration);

            ServiceProvider = services.AddLogging().BuildServiceProvider();

            DbContext = ServiceProvider.GetService<AppDbContext>();
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();

            DbContext.Dispose();
        }
    }
}
