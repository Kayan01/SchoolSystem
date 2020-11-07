using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Shared.AspNetCore;
using Shared.DataAccess.EfCore;
using Shared.DataAccess.EfCore.Context;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.FileStorage;
using Shared.UserManagement;
using Shared.Utils;
using Auth.API.Svc;
using System;
using System.Collections.Generic;
using System.IO;
using Auth.Core.Context;
using Shared.PubSub;
using Microsoft.AspNetCore.Hosting;
using Shared.PubSub.KafkaImpl;
using Auth.Core.EventHandlers;
using Auth.Core.Services.Interfaces;
using Auth.Core.Services;
using Shared.Net.WorkerService;
using Auth.Core.Services.Users;
using Auth.Core.Interfaces.Users;
using Shared.Tenancy;
using Auth.Core.Interfaces;
using Shared.Permissions;
using Microsoft.AspNetCore.Authorization;
using Auth.Core.Services.Interfaces.Class;
using Auth.Core.Services.Class;

namespace Auth.API
{
    public partial class Startup
    {
        public void ConfigureDIService(IServiceCollection services)
        {
            //services.AddScoped<TenantInfo>();

            services.AddTransient<DbContext, AppDbContext>();

            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));

            services.RegisterGenericRepos(typeof(AppDbContext));

            services.AddScoped<IHttpUserService, HttpUserService>();
            services.AddHttpContextAccessor();

            services.AddSingleton<IProducerClient<BusMessage>>(service =>
            {
                var topics = Configuration.GetSection("Kafka").GetValue<string>("Topics").ToString().Split(",");
                var env = service.GetRequiredService<IWebHostEnvironment>();
                var producerClient = new ProducerClient<BusMessage>(env, Configuration);
                return producerClient;
            });

            services.AddSingleton<IConsumerClient<BusMessage>>(service =>
            {
                var topics = Configuration.GetSection("Kafka").GetValue<string>("Topics").ToString().Split(",");
                var env = service.GetRequiredService<IWebHostEnvironment>();
                var consumerClient = new ConsumerClient<BusMessage>(env, Configuration);
                return consumerClient;
            });

            services.AddTransient<Func<List<BusHandler>>>(cont =>
            () =>
            {
                List<BusHandler> handlers = new List<BusHandler>();
                var scope = cont.GetRequiredService<IServiceProvider>().CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<AuthHandler>();
                handlers.Add((message) =>
                {
                    switch (message.BusMessageType)
                    {
                        case (int)BusMessageTypes.TEACHER:
                            {
                                handler.HandleTest(message);
                                break;
                            }
                        case (int)BusMessageTypes.TEACHER_UPDATE:
                            {
                                handler.HandleTest(message);
                                break;
                            }
                    }
                });
                return handlers;
            });

            services.AddSingleton<BoundedMessageChannel<BusMessage>>();
            services.AddHostedService<EventHubProcessorService>();
            services.AddHostedService<EventHubReaderService>();
            services.AddHostedService<PublishMessageBackgroundService>();

            //Permission not needed here
            //services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddScoped<IUserService, UserService>();

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(
                          HostingEnvironment.ContentRootPath, Configuration.GetValue<string>("StoragePath"))));
            services.AddScoped<IBaseRequestAPIService, BaseRequestAPIService>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddScoped<IFileStorageService, FileStorageService>();
            //services.AddTransient<IFileUploadService, FileUploadService>();        }
            services.AddScoped<ITestService, TestService>();

            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<IParentService, ParentService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<ISchoolClassService, SchoolClassService>();
            services.AddScoped<IAuthUserManagement, AuthUserManagementService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IPublishService, PublishService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IClassArmService, ClassArmService>();
            services.AddScoped<IFileStore, FileStore>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddTransient<AuthHandler>();
        }
    }
}
