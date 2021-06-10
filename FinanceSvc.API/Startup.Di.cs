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
using System;
using System.Collections.Generic;
using System.IO;
using FinanceSvc.Core.EventHandlers;
using FinanceSvc.Core.Context;
using Shared.PubSub;
using Microsoft.AspNetCore.Hosting;
using Shared.PubSub.KafkaImpl;
using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.Services;
using Shared.Net.WorkerService;
using Microsoft.Extensions.Logging;

namespace FinanceSvc.API
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
                var handler = scope.ServiceProvider.GetRequiredService<FinanceHandler>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<FinanceHandler>>();
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
                            case (int)BusMessageTypes.PARENT:
                            case (int)BusMessageTypes.PARENT_UPDATE:
                            case (int)BusMessageTypes.PARENT_DELETE:
                                {
                                    await handler.HandleAddOrUpdateParentAsync(message);
                                    break;
                                }
                            case (int)BusMessageTypes.CLASS:
                            case (int)BusMessageTypes.CLASS_UPDATE:
                            case (int)BusMessageTypes.CLASS_DELETE:
                                {
                                    await handler.HandleAddOrUpdateClassAsync(message);
                                    break;
                                }
                            case (int)BusMessageTypes.SESSION:
                            case (int)BusMessageTypes.SESSION_DELETE:
                            case (int)BusMessageTypes.SESSION_UPDATE:
                                {
                                    await handler.HandleAddOrUpdateSessionAsync(message);
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

            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IDocumentService, DocumentService>();

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

            services.AddScoped<IPublishService, PublishService>();

            //services.AddTransient<IFileUploadService, FileUploadService>();
            services.AddScoped<IFinanceService, FinanceService>();
            services.AddTransient<FinanceHandler>();
        }
    }
}
