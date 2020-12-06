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
using AssessmentSvc.API.Svc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AssessmentSvc.Core.Context;
using Shared.PubSub;
using Microsoft.AspNetCore.Hosting;
using Shared.PubSub.KafkaImpl;
using Shared.Net.WorkerService;
using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Services;

namespace AssessmentSvc.API
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

            services.AddSingleton<BoundedMessageChannel<BusMessage>>();
            //services.AddHostedService<EventHubProcessorService>();
            services.AddHostedService<EventHubReaderService>();

            //Permission not needed here
            //services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(
                          HostingEnvironment.ContentRootPath, Configuration.GetValue<string>("StoragePath"))));
            services.AddScoped<IBaseRequestAPIService, BaseRequestAPIService>();

            services.AddScoped<ISessionSetup, SessionService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IAssessmentSetupService, AssessmentSetupService>();
        }
    }
}
