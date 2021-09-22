using Auth.Core.Interfaces;
using Auth.Core.Models;
using Auth.Core.ViewModels.Subscription;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services
{
    public class SubscriptionInvoiceService: ISubscriptionInvoiceService
    {
        public readonly IRepository<SubscriptionInvoice, long> _invoiceRepo;
        public readonly IRepository<SchoolSubscription, long> _subscriptionRepo;
        public readonly IRepository<School, long> _schoolRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionInvoiceService(
            IRepository<SubscriptionInvoice, long> invoiceRepo,
            IRepository<SchoolSubscription, long> subscriptionRepo,
            IRepository<School, long> schoolRepo,
             IUnitOfWork unitOfWork)
        {
            _invoiceRepo = invoiceRepo;
            _subscriptionRepo = subscriptionRepo;
            _schoolRepo = schoolRepo;
            _unitOfWork = unitOfWork;
        }


        public async Task<ResultModel<SubcriptionInvoiceVM>> GetNextSubsciptionInvoice(long schoolId)
        {
            if (schoolId == 0)
            {
                return new ResultModel<SubcriptionInvoiceVM>(errorMessage: "Invalid school Id");
            }

            var schoolSubscriptionInfo = await _schoolRepo.GetAll().Where(m => m.Id == schoolId).Select(m => new {
                SchoolId = m.Id,
                SchoolName = m.Name,
                GroupName = m.SchoolGroup.Name,
                SubscriptionSetup = m.SchoolSubscription,
                LastInvoice = m.SubscriptionInvoices.Where(i=>i.InvoiceType == InvoiceType.Current).OrderBy(n => n.Id).Last(),
                StudentCount = m.Students.Count(n => n.StudentStatusInSchool == Shared.Enums.StudentStatusInSchool.IsStudent),
            }).FirstOrDefaultAsync();

            if (schoolSubscriptionInfo == null)
            {
                return new ResultModel<SubcriptionInvoiceVM>(errorMessage: "School not found");
            }

            if (schoolSubscriptionInfo.SubscriptionSetup == null)
            {
                return new ResultModel<SubcriptionInvoiceVM>(errorMessage: "Subscription setup has not been done for this School.");
            }

            if (schoolSubscriptionInfo.LastInvoice == null)
            {
                var invoiceData = new SubcriptionInvoiceVM()
                {
                    AmountPerStudent = schoolSubscriptionInfo.SubscriptionSetup.PricePerStudent,
                    DueDate = schoolSubscriptionInfo.SubscriptionSetup.EndDate,
                    NumberOfStudent = schoolSubscriptionInfo.SubscriptionSetup.ExpectedNumberOfStudent,
                    SchoolId = schoolSubscriptionInfo.SchoolId,
                    SchoolName = schoolSubscriptionInfo.SchoolName,
                };

                return new ResultModel<SubcriptionInvoiceVM>(data : invoiceData);
            }
            else
            {
                var invoiceData = new SubcriptionInvoiceVM()
                {
                    AmountPerStudent = schoolSubscriptionInfo.SubscriptionSetup.PricePerStudent,
                    DueDate = schoolSubscriptionInfo.SubscriptionSetup.EndDate,
                    NumberOfStudent = schoolSubscriptionInfo.StudentCount,
                    SchoolId = schoolSubscriptionInfo.SchoolId,
                    SchoolName = schoolSubscriptionInfo.SchoolName,
                };

                return new ResultModel<SubcriptionInvoiceVM>(data: invoiceData);
            }
        }

        public async Task<ResultModel<string>> PostNextSubsciptionInvoice(SubcriptionInvoiceVM invoice)
        {
            if (invoice.SchoolId == 0)
            {
                return new ResultModel<string>(errorMessage: "Invalid school Id");
            }

            var schoolSubscriptionInfo = await _schoolRepo.GetAll().Where(m => m.Id == invoice.SchoolId).Select(m => new {
                SchoolId = m.Id,
                SchoolName = m.Name,
                GroupName = m.SchoolGroup.Name,
                SubscriptionSetup = m.SchoolSubscription
            }).FirstOrDefaultAsync();

            if (schoolSubscriptionInfo == null)
            {
                return new ResultModel<string>(errorMessage: "School not found");
            }

            if (schoolSubscriptionInfo.SubscriptionSetup == null)
            {
                return new ResultModel<string>(errorMessage: "Subscription setup has not been done for this School.");
            }

            _invoiceRepo.Insert(new SubscriptionInvoice()
            {
                AmountPerStudent = invoice.AmountPerStudent,
                DueDate = invoice.DueDate,
                InvoiceType = InvoiceType.Current,
                SchoolId = invoice.SchoolId,
                NumberOfStudent = invoice.NumberOfStudent,
                Paid = false,
            });

            await _unitOfWork.SaveChangesAsync();

            //Todo: Send email
            return new ResultModel<string>(data: "Saved successfully");
        }


        public async Task<ResultModel<SubcriptionInvoiceVM>> GetArrearsSubsciptionInvoice(long schoolId)
        {
            if (schoolId == 0)
            {
                return new ResultModel<SubcriptionInvoiceVM>(errorMessage: "Invalid school Id");
            }

            var schoolSubscriptionInfo = await _schoolRepo.GetAll().Where(m => m.Id == schoolId).Select(m => new {
                SchoolId = m.Id,
                SchoolName = m.Name,
                GroupName = m.SchoolGroup.Name,
                SubscriptionSetup = m.SchoolSubscription,
                LastInvoice = m.SubscriptionInvoices.Where(i => i.InvoiceType == InvoiceType.Current).OrderBy(n => n.Id).Last(),
                StudentCount = m.Students.Count(n => n.StudentStatusInSchool == Shared.Enums.StudentStatusInSchool.IsStudent),
            }).FirstOrDefaultAsync();

            if (schoolSubscriptionInfo == null)
            {
                return new ResultModel<SubcriptionInvoiceVM>(errorMessage: "School not found");
            }

            if (schoolSubscriptionInfo.SubscriptionSetup == null)
            {
                return new ResultModel<SubcriptionInvoiceVM>(errorMessage: "Subscription setup has not been done for this School.");
            }

            if (schoolSubscriptionInfo.LastInvoice == null)
            {
                var invoiceData = new SubcriptionInvoiceVM()
                {
                    AmountPerStudent = schoolSubscriptionInfo.SubscriptionSetup.PricePerStudent,
                    DueDate = schoolSubscriptionInfo.SubscriptionSetup.EndDate,
                    NumberOfStudent = schoolSubscriptionInfo.StudentCount - schoolSubscriptionInfo.SubscriptionSetup.ExpectedNumberOfStudent,
                    SchoolId = schoolSubscriptionInfo.SchoolId,
                    SchoolName = schoolSubscriptionInfo.SchoolName,
                };

                return new ResultModel<SubcriptionInvoiceVM>(data: invoiceData);
            }
            else
            {
                var invoiceData = new SubcriptionInvoiceVM()
                {
                    AmountPerStudent = schoolSubscriptionInfo.SubscriptionSetup.PricePerStudent,
                    DueDate = schoolSubscriptionInfo.SubscriptionSetup.EndDate,
                    NumberOfStudent = schoolSubscriptionInfo.StudentCount - schoolSubscriptionInfo.LastInvoice.NumberOfStudent,
                    SchoolId = schoolSubscriptionInfo.SchoolId,
                    SchoolName = schoolSubscriptionInfo.SchoolName,
                };

                return new ResultModel<SubcriptionInvoiceVM>(data: invoiceData);
            }
        }

        public async Task<ResultModel<string>> PostArrearsSubsciptionInvoice(SubcriptionInvoiceVM invoice)
        {
            if (invoice.SchoolId == 0)
            {
                return new ResultModel<string>(errorMessage: "Invalid school Id");
            }

            var schoolSubscriptionInfo = await _schoolRepo.GetAll().Where(m => m.Id == invoice.SchoolId).Select(m => new {
                SchoolId = m.Id,
                SchoolName = m.Name,
                GroupName = m.SchoolGroup.Name,
                SubscriptionSetup = m.SchoolSubscription
            }).FirstOrDefaultAsync();

            if (schoolSubscriptionInfo == null)
            {
                return new ResultModel<string>(errorMessage: "School not found");
            }

            if (schoolSubscriptionInfo.SubscriptionSetup == null)
            {
                return new ResultModel<string>(errorMessage: "Subscription setup has not been done for this School.");
            }

            _invoiceRepo.Insert(new SubscriptionInvoice()
            {
                AmountPerStudent = invoice.AmountPerStudent,
                DueDate = invoice.DueDate,
                InvoiceType = InvoiceType.Arrears,
                SchoolId = invoice.SchoolId,
                NumberOfStudent = invoice.NumberOfStudent,
                Paid = false,
            });

            await _unitOfWork.SaveChangesAsync();

            //Todo: Send email
            return new ResultModel<string>(data: "Saved successfully");
        }


        public async Task<ResultModel<List<GetSubcriptionInvoiceVM>>> GetUnpaidSubsciptionInvoice(long schoolId)
        {
            if (schoolId == 0)
            {
                return new ResultModel<List<GetSubcriptionInvoiceVM>>(errorMessage: "Invalid school Id");
            }

            var invoices = await _invoiceRepo.GetAll().Where(m => m.SchoolId == schoolId && !m.Paid)
                .Select(m=>new GetSubcriptionInvoiceVM() 
                { 
                    AmountPerStudent = m.AmountPerStudent,
                    DueDate = m.DueDate,
                    InvoiceId = m.Id,
                    InvoiceType = m.InvoiceType == InvoiceType.Current ? "Current" : "Arrears",
                    NumberOfStudent = m.NumberOfStudent,
                    Paid = m.Paid,
                    PaidDate = m.PaidDate,
                    SchoolId = m.SchoolId,
                    SchoolName = m.School.Name,
                })
                .ToListAsync();

            return new ResultModel<List<GetSubcriptionInvoiceVM>>(invoices);
        }

        public async Task<ResultModel<string>> MarkInvoiceAsPaid(long invoiceId)
        {
            if (invoiceId == 0)
            {
                return new ResultModel<string>(errorMessage: "Invalid school Id");
            }

            var invoice = await _invoiceRepo.GetAll().FirstOrDefaultAsync(m => m.Id == invoiceId);

            if (invoice is null)
            {
                return new ResultModel<string>(errorMessage: "Invoice not found");
            }

            invoice.Paid = true;
            invoice.PaidDate = DateTime.Today;

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<string>(data: "Saved successfully");
        }

    }
}
