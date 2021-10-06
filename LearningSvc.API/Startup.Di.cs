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
using LearningSvc.API.Svc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LearningSvc.Core.EventHandlers;
using LearningSvc.Core.Context;
using Shared.PubSub;
using Microsoft.AspNetCore.Hosting;
using Shared.PubSub.KafkaImpl;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.Services;
using Shared.Net.WorkerService;
using Shared.Tenancy;
using Microsoft.Extensions.Logging;
using Shared.Infrastructure.HealthChecks;

namespace LearningSvc.API
{
    public partial class Startup
    {
        public void ConfigureDIService(IServiceCollection services)
        {
           //services.AddTransient<TenantInfo>();

           // services.AddTransient<DbContext, AppDbContext>();

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
                var handler = scope.ServiceProvider.GetRequiredService<LearningHandler>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<LearningHandler>>();
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
                            case (int)BusMessageTypes.SCHOOL:
                            case (int)BusMessageTypes.SCHOOL_DELETE:
                            case (int)BusMessageTypes.SCHOOL_UPDATE:
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

            services.AddTransient<LearningHandler>();

            services.AddScoped<IPublishService, PublishService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            //services.AddTransient<IFileUploadService, FileUploadService>(); 
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ITimeTableService, TimeTableService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IAssignmentAnswerService, AssignmentAnswerService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IClassWorkService, ClassWorkService>();
            services.AddScoped<ILessonNoteService, LessonNoteService>();
            services.AddScoped<ITeacherClassSubjectService, TeacherClassSubjectService>();
            services.AddScoped<IClassSubjectService, ClassSubjectService>();
            services.AddScoped<ISchoolClassService, SchoolClassService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<IFileStore, FileStore>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<ZoomService>();

            // Registers required services for health checks
            services.AddCustomHealthChecks(Configuration, "Learning Service");
        }
    }
}
