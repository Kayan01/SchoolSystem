﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NotificationSvc.Core.Context;
using NotificationSvc.Core.Services;
using NotificationSvc.Core.Services.Interfaces;
using NotificationSvc.Core.Test.Mocks;
using Shared.DataAccess.EfCore;
using Shared.DataAccess.EfCore.Context;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.Entities;
using Shared.FileStorage;
using Shared.PubSub;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSvc.Core.Test.Setup
{
    public class ServicesDISetup : IDisposable
    {
        public ServiceCollection services { get; private set; }
        public ServiceProvider ServiceProvider { get; protected set; }
        //public ISchoolService _schoolService;

        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;

        public AppDbContext DbContext { get; set; }

        public ServicesDISetup()
        {
            services = new ServiceCollection();

            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();

            services.AddDbContext<DbContext, AppDbContext>(options => options.UseSqlite(_connection));
            //services.AddDbContextPool<AppDbContext>(options => options.UseSqlite(_connection));

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


            services.AddHttpContextAccessor();
            services.RegisterGenericRepos(typeof(AppDbContext));

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider("/FileStore"));
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IDocumentService, DocumentService>();

            var moqPublish = new MockPublishService();
            services.AddScoped<IPublishService>(_ => moqPublish.Mock.Object);

            var builder = new ConfigurationBuilder()
                //.SetBasePath("path here") //<--You would need to set the path
                .AddJsonFile("appsettings.json"); //or what ever file you have the settings\
            IConfiguration configuration = builder.Build();
            services.AddScoped<IConfiguration>(_ => configuration);

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMailService, SmtpEmailService>();

            ServiceProvider = services.AddLogging().BuildServiceProvider();

            DbContext = ((EfCoreUnitOfWork)ServiceProvider.GetService<IUnitOfWork>()).GetOrCreateDbContext<AppDbContext>();
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();

            DbContext.Dispose();
        }
    }
}
