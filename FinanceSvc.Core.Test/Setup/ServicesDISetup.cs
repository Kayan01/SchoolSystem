using FinanceSvc.Core.Context;
using FinanceSvc.Core.Services;
using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.Test.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
using System.Text;

namespace FinanceSvc.Core.Test.Services.Setup
{
    public class ServicesDISetup : IDisposable
    {
        public ServiceCollection services { get; private set; }
        public ServiceProvider ServiceProvider { get; protected set; }

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

            services.AddScoped<IParentService, ParentService>();
            services.AddScoped<ISchoolClassService, SchoolClassService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IAccountClassService, AccountClassService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountTypeService, AccountTypeService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IComponentService, ComponentService>();
            services.AddScoped<IFeeGroupService, FeeGroupService>();
            services.AddScoped<IFeeComponentService, FeeComponentService>();
            services.AddScoped<IFeeService, FeeService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<ISessionSetupService, SessionSetupService>();
            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IFileStore, FileStore>();

            services.AddScoped<IFinanceService, FinanceService>();



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
