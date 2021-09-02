﻿using AssessmentSvc.Core.Context;
using AssessmentSvc.Core.EventHandlers;
using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Services;
using AssessmentSvc.Core.Test.Mocks;
using AssessmentSvc.Core.Utils;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Shared.AspNetCore;
using Shared.DataAccess.EfCore;
using Shared.DataAccess.EfCore.Context;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.Entities;
using Shared.FileStorage;
using Shared.PubSub;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AssessmentSvc.Core.Test.Setup
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

            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<ISessionSetup, SessionService>();
            services.AddScoped<IAssessmentSetupService, AssessmentSetupService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IHttpUserService, HttpUserService>();
            services.AddScoped<ISchoolClassService, SchoolClassService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IResultService, ResultService>();
            services.AddScoped<IGradeSetupService, GradeSetupService>();
            services.AddScoped<IApprovedResultService, ApprovedResultService>();
            services.AddScoped<IIncidenceService, IncidenceService>();
            services.AddScoped<IPromotionSetupService, PromotionSetupService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IResultSummaryService, ResultSummaryService>();

            var moqToPDF = new MockToPDF();
            services.AddSingleton<IToPDF>(_ => moqToPDF.Mock.Object);

            var httpClientFactory = new MockHttpClientFactory();
            services.AddScoped<IHttpClientFactory>(_ => httpClientFactory.Mock.Object);



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
