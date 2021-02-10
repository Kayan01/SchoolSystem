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
using FacilitySvc.API.Svc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FacilitySvc.Core.EventHandlers;
using FacilitySvc.Core.Context;
using Shared.PubSub;
using Microsoft.AspNetCore.Hosting;
using Shared.PubSub.KafkaImpl;
using FacilitySvc.Core.Services.Interfaces;
using FacilitySvc.Core.Services;
using Shared.Net.WorkerService;
using Microsoft.Extensions.Logging;

namespace FacilitySvc.API
{
    public partial class Startup
    {
        public void ConfigureDIService(IServiceCollection services)
        {
            services.AddTransient<DbContext, AppDbContext>();

            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));

            services.RegisterGenericRepos(typeof(AppDbContext));

            services.AddScoped<IHttpUserService, HttpUserService>();
            services.AddHttpContextAccessor();

            services.AddSingleton<IProducerClient<BusMessage>>(service =>
            {
                var env = service.GetRequiredService<IWebHostEnvironment>();
                var producerClient = new ProducerClient<BusMessage>(env, Configuration);
                return producerClient;
            });

            services.AddSingleton<IConsumerClient<BusMessage>>(service =>
            {
                var env = service.GetRequiredService<IWebHostEnvironment>();
                var consumerClient = new ConsumerClient<BusMessage>(env, Configuration);
                return consumerClient;
            });

            services.AddTransient<Func<List<BusHandler>>>(cont =>
            () =>
            {
                List<BusHandler> handlers = new List<BusHandler>();
                var scope = cont.GetRequiredService<IServiceProvider>().CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<NoticeHandler>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<NoticeHandler>>();
                handlers.Add((message) =>
                {
                    try
                    {
                        switch (message.BusMessageType)
                        {
                            case (int)BusMessageTypes.NOTIFICATION:
                                {
                                    handler.HandleTest(message);
                                    break;
                                }
                            case (int)BusMessageTypes.TEACHER:
                                {
                                    handler.HandleTest(message);
                                    break;
                                }
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e.Message, e);
                    }
                });
                return handlers;
            });
            services.AddSingleton<BoundedMessageChannel<BusMessage>>();
            services.AddHostedService<EventHubProcessorService>();
            services.AddHostedService<EventHubReaderService>();

            //Permission not needed here
            //services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(
                          HostingEnvironment.ContentRootPath, Configuration.GetValue<string>("StoragePath"))));
            services.AddScoped<IBaseRequestAPIService, BaseRequestAPIService>();

            services.AddScoped<IFileStorageService, FileStorageService>();
            //services.AddTransient<IFileUploadService, FileUploadService>();     
            services.AddScoped<INotificationService, NotificationService>();
            services.AddTransient<NoticeHandler>();
        }
    }
}
