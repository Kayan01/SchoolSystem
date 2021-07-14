using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Shared.AspNetCore;
using Shared.DataAccess.EfCore;
using Shared.DataAccess.EfCore.Context;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.FileStorage;
using Shared.Utils;
using AssessmentSvc.API.Svc;
using System;
using System.Collections.Generic;
using System.IO;
using AssessmentSvc.Core.Context;
using Shared.PubSub;
using Microsoft.AspNetCore.Hosting;
using Shared.PubSub.KafkaImpl;
using Shared.Net.WorkerService;
using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Services;
using AssessmentSvc.Core.EventHandlers;
using Microsoft.Extensions.Logging;
using Shared.Infrastructure.HealthChecks;
using AssessmentSvc.Core.Utils;

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
                var handler = scope.ServiceProvider.GetRequiredService<AssessmentHandler>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<AssessmentHandler>>();
                handlers.Add(async (message) =>
                {
                    try
                    {
                        switch (message.BusMessageType)
                        {
                            case (int)BusMessageTypes.STUDENT:
                            case (int)BusMessageTypes.STUDENT_UPDATE:
                            case (int)BusMessageTypes.STUDENT_DELETE:
                                {
                                    await handler.HandleAddOrUpdateStudentAsync(message);
                                    break;
                                }
                            case (int)BusMessageTypes.TEACHER:
                            case (int)BusMessageTypes.TEACHER_UPDATE:
                            case (int)BusMessageTypes.TEACHER_DELETE:
                                {
                                    await handler.HandleAddOrUpdateTeacherAsync(message);
                                    break;
                                }
                            case (int)BusMessageTypes.CLASS:
                            case (int)BusMessageTypes.CLASS_UPDATE:
                            case (int)BusMessageTypes.CLASS_DELETE:
                                {
                                    await handler.HandleAddOrUpdateClassAsync(message);
                                    break;
                                }
                            case (int)BusMessageTypes.SUBJECT:
                            case (int)BusMessageTypes.SUBJECT_UPDATE:
                            case (int)BusMessageTypes.SUBJECT_DELETE:
                                {
                                    await handler.HandleAddOrUpdateSubjectAsync(message);
                                    break;
                                }
                            case (int)BusMessageTypes.SCHOOL:
                            case (int)BusMessageTypes.SCHOOL_UPDATE:
                            case (int)BusMessageTypes.SCHOOL_DELETE:
                                {
                                    await handler.HandleAddOrUpdateSchoolAsync(message);
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

            Directory.CreateDirectory(Path.Combine(HostingEnvironment.ContentRootPath, Configuration.GetValue<string>("StoragePath")));
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(
                          HostingEnvironment.ContentRootPath, Configuration.GetValue<string>("StoragePath"))));
            services.AddScoped<IBaseRequestAPIService, BaseRequestAPIService>();

            services.AddTransient<AssessmentHandler>();

            services.AddScoped<IPublishService, PublishService>();

            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<ISessionSetup, SessionService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IAssessmentSetupService, AssessmentSetupService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ISchoolClassService, SchoolClassService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IResultService, ResultService>();
            services.AddScoped<IGradeSetupService, GradeSetupService>();
            services.AddScoped<IApprovedResultService, ApprovedResultService>();
            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<IIncidenceService, IncidenceService>();
            services.AddScoped<IPromotionSetupService, PromotionSetupService>();

            services.AddScoped<IToPDF, ToPDF>();

            services.AddHttpClient("localclient", c =>
            {
                c.BaseAddress = new Uri(Configuration["AuthBaseUrl"]);
            });

            // Registers required services for health checks
            services.AddCustomHealthChecks(Configuration, "Assessment Service");
        }
    }
}
