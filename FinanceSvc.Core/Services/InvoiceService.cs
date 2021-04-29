using DinkToPdf.Contracts;
using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels;
using FinanceSvc.Core.ViewModels.Invoice;
using IPagedList;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.FileStorage;
using Shared.Pagination;
using Shared.PubSub;
using Shared.Utils;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Utils.CoreConstants;

namespace FinanceSvc.Core.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IRepository<Invoice, long> _invoiceRepo;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IRepository<SessionSetup, long> _sessionRepo;
        private readonly IRepository<School, long> _schoolRepo;
        private readonly IRepository<Fee, long> _feeRepo;
        private readonly IConverter _converter;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IFileStorageService _fileStorageService;
        private readonly IPublishService _publishService;
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork,
            IRepository<Invoice, long> invoiceRepo,
            IRepository<Fee, long> feeRepo,
            IRepository<Student, long> studentRepo,
       IRepository<SessionSetup, long> sessionRepo,
       IWebHostEnvironment hostingEnvironment,
       IFileStorageService fileStorageService,
       IPublishService publishService,
       IRepository<School, long> schoolRepo,
       IConverter converter)
        {
            _unitOfWork = unitOfWork;
            _invoiceRepo = invoiceRepo;
            _feeRepo = feeRepo;
            _studentRepo = studentRepo;
            _sessionRepo = sessionRepo;
            _hostingEnvironment = hostingEnvironment;
            _fileStorageService = fileStorageService;
            _publishService = publishService;
            _converter = converter;
            _schoolRepo = schoolRepo;
        }

        public async Task<ResultModel<string>> AddInvoice(InvoicePostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _invoiceRepo.GetAll().Where(m =>
                m.Student.ClassId == model.ClassId &&
                m.Fee.FeeGroupId == model.FeeGroupId &&
                m.SessionSetupId == model.SessionId &&
                m.TermSequenceNumber == model.TermSequence
                ).FirstOrDefaultAsync();

            if (check != null)
            {
                result.AddError("Invoice has been generated for this class before!");
                return result;
            }

            var fee = await _feeRepo.GetAll().Include(m => m.FeeComponents).ThenInclude(n => n.Component).Where(m => m.FeeGroupId == model.FeeGroupId && m.SchoolClassId == model.ClassId).FirstOrDefaultAsync();

            if (fee is null)
            {
                result.AddError("No Fee has been created for this Fee Group and Class");
                return result;
            }

            var students = await _studentRepo.GetAll().Where(m => m.ClassId == model.ClassId).Select(m => new
            {
                m.Id,
                ClassName = $"{ m.Class.Name} {m.Class.ClassArm}",
                FullName = $"{m.FirstName} {m.LastName}",
                ParentEmail = m.Parent.Email,
                SchoolId = m.TenantId
            }).ToListAsync();

            if (students?.Count < 1)
            {
                result.AddError("No Student was found in this Class.");
                return result;
            }
            var session = await _sessionRepo.GetAll().FirstOrDefaultAsync(x => x.Id == model.SessionId);


            var emailData = new List<InvoicePdfVM>();

            var school = await _schoolRepo.GetAll().FirstOrDefaultAsync(x => x.Id == students.First().SchoolId);

            foreach (var student in students)
            {
                var invoice = new Invoice()
                {
                    ApprovalStatus = Enumerations.InvoiceApprovalStatus.Approved,
                    FeeId = fee.Id,
                    SessionSetupId = model.SessionId,
                    PaymentDate = model.PaymentDate,
                    StudentId = student.Id,
                    PaymentStatus = Enumerations.InvoicePaymentStatus.Unpaid,
                    TermSequenceNumber = model.TermSequence,
                    ComponentSelected = false,
                    InvoiceComponents = fee.FeeComponents.Select(m => new InvoiceComponent()
                    {
                        Amount = m.Amount,
                        ComponentName = m.Component.Name,
                        IsSelected = true,
                        IsCompulsory = m.IsCompulsory,
                    }).ToList()
                };

                var invoiceId = await _invoiceRepo.InsertAndGetIdAsync(invoice);

                //get invoice email data
                var mainData = new
                {
                    TotalAmount = fee.FeeComponents.Sum(x => x.Amount),
                    termName = model.TermSequence,
                    session = session?.SessionName,
                    InvoiceDueDate = model.PaymentDate.ToLongDateString(),
                    invoiceDate = DateTime.Now.Date.ToLongDateString(),
                    invoiceNumber = invoiceId.ToString("000000"),
                    className = student.ClassName,
                    studentName = student.FullName,
                    SchoolLogoPath = school?.Logo
                };

                var listData = (IEnumerable<dynamic>)fee.FeeComponents.Select(x => new { Description = x.Component.Name, Compulsory = x.IsCompulsory, Amount = x.Amount }).ToList();

                var kv = new KeyValuePair<dynamic, IEnumerable<dynamic>>(mainData, listData);
                var item = new InvoicePdfVM { ParentEmail = student.ParentEmail, KeyValuePair = kv , StudentName = student.FullName};
                emailData.Add(item);
            }

            await _unitOfWork.SaveChangesAsync();

            await SendInvoicePdf(emailData);

            result.Data = "Saved successfully";
            return result;
        }

        public async Task<ResultModel<InvoiceDetailVM>> GetInvoice(long id)
        {
            var result = new ResultModel<InvoiceDetailVM>();

            result.Data = await _invoiceRepo.GetAll().Where(m => m.Id == id).Select(m => new InvoiceDetailVM()
            {
                InvoiceId = m.Id,
                StudentName = $"{m.Student.FirstName} {m.Student.LastName}",
                Session = m.SessionSetup.SessionName,
                TermSequence = m.TermSequenceNumber,
                TermsJSON = m.SessionSetup.TermsJSON,
                StudentRegNumber = m.Student.RegNumber,
                approvalStatus = m.ApprovalStatus,
                paymentStatus = m.PaymentStatus,
                DueDate = m.PaymentDate,
                Class = $"{m.Fee.SchoolClass.Name} {m.Fee.SchoolClass.ClassArm}",
                FeeGroup = m.Fee.FeeGroup.Name,
                ComponentSelected = m.ComponentSelected,
                CreationDate = m.CreationTime,
                InvoiceItems = m.InvoiceComponents.Select(n => new InvoiceItemVM()
                {
                    Id = n.Id,
                    Amount = n.Amount,
                    Name = n.ComponentName,
                    IsCompulsory = n.IsCompulsory,
                    IsSelected = n.IsSelected
                }).ToList(),
                TotalPaid = m.Transactions.Where(m =>
                    m.Status == Enumerations.TransactionStatus.Paid ||
                    m.Status == Enumerations.TransactionStatus.Awaiting_Approval
                    ).Sum(n => n.Amount),
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ResultModel<string>> UpdateInvoiceComponentSelection(InvoiceComponentSelectionUpdateVM vm)
        {
            var invoice = await _invoiceRepo.GetAll().Include(m => m.InvoiceComponents)
                .Where(m => m.Id == vm.InvoiceId).FirstOrDefaultAsync();

            if (invoice is null)
            {
                return new ResultModel<string>("Invoice not found");
            }

            if (invoice.ComponentSelected)
            {
                return new ResultModel<string>(errorMessage: "Components can only be selected once.");
            }

            if (invoice.InvoiceComponents.Count != vm.ComponentSelections.Count)
            {
                return new ResultModel<string>(errorMessage: "Component count mis-match. Please check that you are sending all the invoice components.");
            }

            foreach (var item in invoice.InvoiceComponents)
            {
                var vmComponent = vm.ComponentSelections.FirstOrDefault(m => m.ComponentId == item.Id);
                if (vmComponent is null)
                {
                    return new ResultModel<string>(errorMessage: "One or more invoice is missing.");
                }

                if (!item.IsCompulsory)
                {
                    item.IsSelected = vmComponent.IsSelected;
                }
            }

            invoice.ComponentSelected = true;

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<string>(data: "Updated Successfully.");
        }

        public async Task<ResultModel<PaginatedModel<InvoicePaymentVM>>> GetPaymentInvoices(long? sessionId, int? termSequence, QueryModel queryModel)
        {
            var all = _invoiceRepo.GetAll();

            if (!(sessionId is null || termSequence is null))
            {
                all = all.Where(n => n.SessionSetupId == sessionId && n.TermSequenceNumber == termSequence);
            }

            var query = await all.Select(m => new InvoicePaymentVM()
            {
                DueDate = m.PaymentDate,
                FeeGroup = m.Fee.FeeGroup.Name,
                StudentRegNumber = m.Student.RegNumber,
                PaymentStatus = m.PaymentStatus,
                InvoiceId = m.Id,
                Total = m.InvoiceComponents.Where(m => m.IsSelected).Sum(n => n.Amount),
                Paid = m.Transactions.Sum(n => n.Amount),
            })
                .ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            return new ResultModel<PaginatedModel<InvoicePaymentVM>>
            {
                Data = new PaginatedModel<InvoicePaymentVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
        }

        public async Task<ResultModel<PaginatedModel<InvoicePaymentHistoryVM>>> GetPaymentHistoryInvoices(long? sessionId, int? termSequence, QueryModel queryModel)
        {
            var all = _invoiceRepo.GetAll();

            if (!(sessionId is null || termSequence is null))
            {
                all = all.Where(n => n.SessionSetupId == sessionId && n.TermSequenceNumber == termSequence);
            }

            var query = await all.Select(m => new InvoicePaymentHistoryVM()
            {
                DueDate = m.PaymentDate,
                FeeGroup = m.Fee.FeeGroup.Name,
                StudentRegNumber = m.Student.RegNumber,
                InvoiceId = m.Id,
                Total = m.InvoiceComponents.Where(m => m.IsSelected).Sum(n => n.Amount),
                Session = m.SessionSetup.SessionName,
                TermSequence = m.TermSequenceNumber,
                TermsJSON = m.SessionSetup.TermsJSON,
            })
                .ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            return new ResultModel<PaginatedModel<InvoicePaymentHistoryVM>>
            {
                Data = new PaginatedModel<InvoicePaymentHistoryVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
        }

        public async Task<ResultModel<PaginatedModel<InvoicePaymentVM>>> GetInvoices(InvoiceRequestVM model, QueryModel queryModel)
        {
            var invoiceQuery = _invoiceRepo.GetAll();

            if (!(model.ClassId is null))
            {
                invoiceQuery = invoiceQuery.Where(m => m.Fee.SchoolClassId == model.ClassId);
            }
            if (!(model.PaymentStatus is null))
            {
                invoiceQuery = invoiceQuery.Where(m => m.PaymentStatus == model.PaymentStatus);
            }
            if (!(model.SessionId is null))
            {
                invoiceQuery = invoiceQuery.Where(m => m.SessionSetupId == model.SessionId);
            }
            if (!(model.TermSequence is null))
            {
                invoiceQuery = invoiceQuery.Where(m => m.TermSequenceNumber == model.TermSequence);
            }
            if (!(model.StudentId is null))
            {
                invoiceQuery = invoiceQuery.Where(m => m.StudentId == model.StudentId);
            }


            var query = await invoiceQuery.Select(m => new InvoicePaymentVM()
            {
                DueDate = m.PaymentDate,
                FeeGroup = m.Fee.FeeGroup.Name,
                StudentRegNumber = m.Student.RegNumber,
                InvoiceId = m.Id,
                Total = m.InvoiceComponents.Where(m => m.IsSelected).Sum(n => n.Amount),
                Paid = m.Transactions.Where(m => m.Status == Enumerations.TransactionStatus.Paid).Sum(n => n.Amount),
                PaymentStatus = m.PaymentStatus
            })
                .ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            return new ResultModel<PaginatedModel<InvoicePaymentVM>>
            {
                Data = new PaginatedModel<InvoicePaymentVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
        }

        public async Task<ResultModel<PaginatedModel<InvoiceVM>>> GetAllInvoices(long? sessionId, int? termSequence, QueryModel queryModel)
        {
            var all = _invoiceRepo.GetAll();

            if (!(sessionId is null || termSequence is null))
            {
                all = all.Where(n => n.SessionSetupId == sessionId && n.TermSequenceNumber == termSequence);
            }

            var query = await all.Select(m => new InvoiceVM()
            {
                DueDate = m.PaymentDate,
                FeeGroup = m.Fee.FeeGroup.Name,
                StudentRegNumber = m.Student.RegNumber,
                InvoiceId = m.Id,
                Total = m.InvoiceComponents.Where(m => m.IsSelected).Sum(n => n.Amount),
                Class = $"{m.Fee.SchoolClass.Name} {m.Fee.SchoolClass.ClassArm}",
                ApprovalStatus = m.ApprovalStatus,
                Session = m.SessionSetup.SessionName,
                TermsJSON = m.SessionSetup.TermsJSON,
                TermSequence = m.TermSequenceNumber,
                CreationDate = m.CreationTime,
                InvoiceItems = m.InvoiceComponents.Select(n => new InvoiceItemVM()
                {
                    Id = n.Id,
                    Amount = n.Amount,
                    Name = n.ComponentName,
                    IsCompulsory = n.IsCompulsory,
                    IsSelected = n.IsSelected
                }).ToList(),
                StudentName = $"{m.Student.FirstName} {m.Student.LastName}",
            })
                .ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            return new ResultModel<PaginatedModel<InvoiceVM>>
            {
                Data = new PaginatedModel<InvoiceVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
        }

        public async Task<ResultModel<PaginatedModel<InvoicePendingPaymentVM>>> GetPendingPaymentInvoices(long? sessionId, int? termSequence, QueryModel queryModel)
        {
            var all = _invoiceRepo.GetAll();

            if (!(sessionId is null || termSequence is null))
            {
                all = all.Where(n => n.SessionSetupId == sessionId && n.TermSequenceNumber == termSequence);
            }

            var query = await _invoiceRepo.GetAll().Where(n =>
                n.SessionSetupId == sessionId &&
                n.TermSequenceNumber == termSequence &&
                n.ApprovalStatus == Enumerations.InvoiceApprovalStatus.Approved &&
                n.PaymentStatus != Enumerations.InvoicePaymentStatus.Paid)
            .Select(m => new InvoicePendingPaymentVM()
            {
                DueDate = m.PaymentDate,
                FeeGroup = m.Fee.FeeGroup.Name,
                StudentRegNumber = m.Student.RegNumber,
                InvoiceId = m.Id,
                Total = m.InvoiceComponents.Where(m => m.IsSelected).Sum(n => n.Amount),
                ApprovalStatus = m.ApprovalStatus,
                Session = m.SessionSetup.SessionName,
                TermsJSON = m.SessionSetup.TermsJSON,
                TermSequence = m.TermSequenceNumber,
            })
                .ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            return new ResultModel<PaginatedModel<InvoicePendingPaymentVM>>
            {
                Data = new PaginatedModel<InvoicePendingPaymentVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
        }

        public async Task SendInvoicePdf(List<InvoicePdfVM> data)
        {
            
            var pdfsaved = new List<KeyValuePair<InvoicePdfVM, string>>();
            var tableConfig = new TableAttributeConfig
            {
                TableAttributes = new { @style = "width: 100%; border-collapse: unset; border-spacing: 0;" },
                ThAttributes = new { @style = "text-align: left; font-size: 11.2247px; line-height: 133.9%; color: #111118; font-weight: bold;border-bottom: 1px solid #111118 ;" },
                TdAttributes = new { },
            };

            var templatePath = _fileStorageService.MapStorage(CoreConstants.InvoicePdfTemplatePath);
            foreach (var item in data)
            {
               var pdf = _converter.ConvertToPDFBytesToList((object)item.KeyValuePair.Key, item.KeyValuePair.Value.ToList(), tableConfig, templatePath, false);

                var path = $"invoices\\{Guid.NewGuid()}.pdf";
                _fileStorageService.SaveBytes(path, pdf);


                pdfsaved.Add(new KeyValuePair<InvoicePdfVM,string>( new InvoicePdfVM {ParentEmail = item.ParentEmail, StudentName = item.StudentName }, path));
            }

            await _publishService.PublishMessage(Topics.Notification, BusMessageTypes.NOTIFICATION, new CreateNotificationModel
            {
                Emails = pdfsaved.Select(m => new CreateEmailModel(
                   EmailTemplateType.Invoice,
                   new Dictionary<string, string> { { "studentName", m.Key.StudentName } },
                   new UserVM() { Email = m.Key.ParentEmail },
                   new List<string> { m.Value }
                )).ToList()
            });
        }

      
    }
}
