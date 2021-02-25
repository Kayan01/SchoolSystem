using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.Invoice;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IRepository<Invoice, long> _invoiceRepo;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IRepository<Fee, long> _feeRepo;
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork, 
            IRepository<Invoice, long> invoiceRepo, 
            IRepository<Fee, long> feeRepo,
            IRepository<Student, long> studentRepo)
        {
            _unitOfWork = unitOfWork;
            _invoiceRepo = invoiceRepo;
            _feeRepo = feeRepo;
            _studentRepo = studentRepo;
        }

        public async Task<ResultModel<string>> AddInvoice(InvoicePostVM model)
        {
            var result = new ResultModel<string>();

            var check = await _invoiceRepo.GetAll().Where(m => 
                m.Student.ClassId == model.ClassId && 
                m.Fee.FeeGroupId == model.FeeGroupId && 
                m.Session == model.Session &&
                m.Term == model.Term
                ).FirstOrDefaultAsync();

            if (check != null)
            {
                result.AddError("Invoice has been generated for this class before!");
                return result;
            }

            var fee = await _feeRepo.GetAll().Where(m => m.FeeGroupId == model.FeeGroupId && m.SchoolClassId == model.ClassId).FirstOrDefaultAsync();
            var studentIds = await _studentRepo.GetAll().Where(m => m.ClassId == model.ClassId).Select(m=>m.Id).ToListAsync();

            foreach (var studentId in studentIds)
            {
                var invoice = new Invoice()
                {
                    ApprovalStatus = Enumerations.InvoiceApprovalStatus.Approved,
                    FeeId = fee.Id,
                    Session = model.Session,
                    PaymentDate = model.PaymentDate,
                    StudentId = studentId,
                    PaymentStatus = Enumerations.InvoicePaymentStatus.Unpaid,
                    Term = model.Term
                };

                _invoiceRepo.Insert(invoice);
            }

            await _unitOfWork.SaveChangesAsync();

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
                Session = m.Session,
                StudentRegNumber = m.Student.RegNumber,
                approvalStatus = m.ApprovalStatus,
                paymentStatus = m.PaymentStatus,
                DueDate = m.PaymentDate,
                Class = $"{m.Fee.SchoolClass.Name} {m.Fee.SchoolClass.ClassArm}",
                FeeGroup = m.Fee.FeeGroup.Name,
                InvoiceItems = m.Fee.FeeComponents.Select(n => new InvoiceItemVM()
                {
                    Amount = n.Amount,
                    Name = n.Component.Name,
                }).ToList(),
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ResultModel<List<InvoicePaymentVM>>> GetPaymentInvoices(string session, string term)
        {
            var result = new ResultModel<List<InvoicePaymentVM>>();

            result.Data = await _invoiceRepo.GetAll().Where(n=>n.Session == session && n.Term == term).Select(m => new InvoicePaymentVM()
            {
                DueDate = m.PaymentDate,
                FeeGroup = m.Fee.FeeGroup.Name,
                StudentRegNumber=m.Student.RegNumber,
                PaymentStatus=m.PaymentStatus,
                InvoiceId = m.Id,
                Total = m.Fee.FeeComponents.Sum(n=>n.Amount),
                Paid = m.Payments.Sum(n=> n.AmountPaid),
            }).ToListAsync();

            return result;
        }

        public async Task<ResultModel<List<InvoicePaymentHistoryVM>>> GetPaymentHistoryInvoices(string session, string term)
        {
            var result = new ResultModel<List<InvoicePaymentHistoryVM>>();

            result.Data = await _invoiceRepo.GetAll().Where(n => n.Session == session && n.Term == term).Select(m => new InvoicePaymentHistoryVM()
            {
                DueDate = m.PaymentDate,
                FeeGroup = m.Fee.FeeGroup.Name,
                StudentRegNumber = m.Student.RegNumber,
                InvoiceId = m.Id,
                Total = m.Fee.FeeComponents.Sum(n => n.Amount),
                Session = m.Session,
                Term = m.Term
            }).ToListAsync();

            return result;
        }

        public async Task<ResultModel<List<InvoicePaymentVM>>> GetInvoices(InvoiceRequestVM model)
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
            if (!string.IsNullOrWhiteSpace(model.Session))
            {
                invoiceQuery = invoiceQuery.Where(m => m.Session == model.Session);
            }
            if (!string.IsNullOrWhiteSpace(model.Term))
            {
                invoiceQuery = invoiceQuery.Where(m => m.Term == model.Term);
            }
            if (!(model.StudentId is null))
            {
                invoiceQuery = invoiceQuery.Where(m => m.StudentId == model.StudentId);
            }


            var result = new ResultModel<List<InvoicePaymentVM>>();

            result.Data = await invoiceQuery.Select(m => new InvoicePaymentVM()
            {
                DueDate = m.PaymentDate,
                FeeGroup = m.Fee.FeeGroup.Name,
                StudentRegNumber = m.Student.RegNumber,
                InvoiceId = m.Id,
                Total = m.Fee.FeeComponents.Sum(n => n.Amount),
                PaymentStatus = m.PaymentStatus,
                Paid = m.Payments.Sum(m=> m.AmountPaid),
            }).ToListAsync();

            return result;
        }

        public async Task<ResultModel<List<InvoiceVM>>> GetAllInvoices(string session, string term)
        {
            var result = new ResultModel<List<InvoiceVM>>();

            result.Data = await _invoiceRepo.GetAll().Where(n => n.Session == session && n.Term == term).Select(m => new InvoiceVM()
            {
                DueDate = m.PaymentDate,
                FeeGroup = m.Fee.FeeGroup.Name,
                StudentRegNumber = m.Student.RegNumber,
                InvoiceId = m.Id,
                Total = m.Fee.FeeComponents.Sum(n => n.Amount),
                Session = m.Session,
                Term = m.Term,
                Class = $"{m.Student.Class.Name} {m.Student.Class.ClassArm}",
                ApprovalStatus = m.ApprovalStatus,
            }).ToListAsync();

            return result;
        }

        public async Task<ResultModel<List<InvoicePendingPaymentVM>>> GetPendingPaymentInvoices(string session, string term)
        {
            var result = new ResultModel<List<InvoicePendingPaymentVM>>();

            result.Data = await _invoiceRepo.GetAll().Where(n => n.Session == session && n.Term == term && 
                n.ApprovalStatus == Enumerations.InvoiceApprovalStatus.Approved && n.PaymentStatus != Enumerations.InvoicePaymentStatus.Paid)
                .Select(m => new InvoicePendingPaymentVM()
            {
                DueDate = m.PaymentDate,
                FeeGroup = m.Fee.FeeGroup.Name,
                StudentRegNumber = m.Student.RegNumber,
                InvoiceId = m.Id,
                Total = m.Fee.FeeComponents.Sum(n => n.Amount),
                Session = m.Session,
                Term = m.Term,
                ApprovalStatus = m.ApprovalStatus,
            }).ToListAsync();

            return result;
        }
    }
}
