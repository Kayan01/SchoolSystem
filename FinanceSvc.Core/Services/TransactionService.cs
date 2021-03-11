using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.Transaction;
using IPagedList;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.FileStorage;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository<Transaction, long> _transactionRepo;
        private readonly IRepository<Invoice, long> _invoiceRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentService _documentService;

        public TransactionService(IUnitOfWork unitOfWork,
            IRepository<Invoice, long> invoiceRepo, IDocumentService documentService,
            IRepository<Transaction, long> transactionRepo)
        {
            _unitOfWork = unitOfWork;
            _transactionRepo = transactionRepo;
            _invoiceRepo = invoiceRepo;
            _documentService = documentService;
        }

        public async Task<ResultModel<string>> NewPendingTransaction(TransactionPostVM model)
        {
            var check = await _invoiceRepo.GetAll()
                .Include(m => m.InvoiceComponents)
                .Include(n => n.Transactions)
                .SingleOrDefaultAsync(m => m.Id == model.InvoiceId);

            if (check == null)
            {
                return new ResultModel<string>(errorMessage: "Invoice was not found.");
            }

            if (!check.ComponentSelected)
            {
                return new ResultModel<string>(errorMessage: "Please make sure you have selected items to be paid.");
            }

            if (check.InvoiceComponents?.Sum(m => m.Amount) < (check.Transactions?.Sum(m => m.Amount) + model.Amount))
            {
                return new ResultModel<string>(errorMessage: "Amount to be paid would be greater than the Amount payable.");
            }

            var transaction = new Transaction()
            {
                InvoiceId = model.InvoiceId,
                Status = Enumerations.TransactionStatus.Pending,
                Amount = model.Amount,
                Description = model.Description,
            };

            _transactionRepo.Insert(transaction);

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<string>(data: "Saved successfully");
        }

        public async Task<ResultModel<List<TransactionVM>>> GetAllPendingTransactions(long studentId)
        {
            return new ResultModel<List<TransactionVM>>
                (
                    data: await _transactionRepo.GetAll()
                        .Where(n => n.Invoice.StudentId == studentId && n.Status == Enumerations.TransactionStatus.Pending)
                        .Select(m => new TransactionVM()
                        {
                            Description = m.Description,
                            Amount = m.Amount,
                            InvoiceId = m.InvoiceId,
                            status = m.Status,
                            TransactionId = m.Id,
                            DueDate = m.Invoice.PaymentDate,
                            FeeType = m.Invoice.Fee.Name,
                        }
                    ).ToListAsync()
                );
        }

        public async Task<ResultModel<List<TransactionVM>>> GetAllAwaitingApprovalTransactions()
        {
            return new ResultModel<List<TransactionVM>>
                (
                    data: await _transactionRepo.GetAll()
                        .Where(n => n.Status == Enumerations.TransactionStatus.Awaiting_Approval)
                        .Select(m => new TransactionVM()
                        {
                            Description = m.Description,
                            Amount = m.Amount,
                            InvoiceId = m.InvoiceId,
                            status = m.Status,
                            TransactionId = m.Id,
                            DueDate = m.Invoice.PaymentDate,
                            FeeType = m.Invoice.Fee.Name,
                            FileId = m.FileUploadId.ToString(),
                        }
                    ).ToListAsync()
                );
        }

        public async Task<ResultModel<PaginatedModel<TransactionVM>>> GetAllTransactions(QueryModel queryModel)
        {
            var query = await _transactionRepo.GetAll()
                        .Where(n => n.Status == Enumerations.TransactionStatus.Awaiting_Approval)
                        .Select(m => new TransactionVM()
                        {
                            Description = m.Description,
                            Amount = m.Amount,
                            InvoiceId = m.InvoiceId,
                            status = m.Status,
                            TransactionId = m.Id,
                            DueDate = m.Invoice.PaymentDate,
                            FeeType = m.Invoice.Fee.Name,
                            FileId = m.FileUploadId.ToString(),
                        }
                    )
                    .ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            return new ResultModel<PaginatedModel<TransactionVM>>
                {
                    Data = new PaginatedModel<TransactionVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
                };
        }

        public async Task<ResultModel<List<TransactionVM>>> GetTransactionHistory(long studentId)
        {
            return new ResultModel<List<TransactionVM>>
                (
                    data: await _transactionRepo.GetAll()
                        .Where(n =>
                            n.Invoice.StudentId == studentId &&
                            (
                                n.Status == Enumerations.TransactionStatus.Rejected ||
                                n.Status == Enumerations.TransactionStatus.Paid
                            )
                        )
                        .Select(m => new TransactionVM()
                        {
                            Description = m.Description,
                            Amount = m.Amount,
                            InvoiceId = m.InvoiceId,
                            status = m.Status,
                            TransactionId = m.Id,
                            DueDate = m.Invoice.PaymentDate,
                            FeeType = m.Invoice.Fee.Name,
                        }
                    ).ToListAsync()
                );
        }

        public async Task<ResultModel<string>> UploadTransactionReceipt(TransactionReceiptVM model)
        {
            var transaction = await _transactionRepo.GetAll().SingleOrDefaultAsync(m => m.Id == model.TransactionId);

            if (transaction == null)
            {
                return new ResultModel<string>(errorMessage: "Transaction was not found.");
            }

            if (!(transaction.FileUploadId is null))
            {
                return new ResultModel<string>(errorMessage: "Document has already been uploaded");
            }

            //save file
            var file = await _documentService.TryUploadSupportingDocument(model.Document, Shared.Enums.DocumentType.Transaction_Receipt);

            if (file == null)
            {
                return new ResultModel<string>(errorMessage: "File could not be uploaded");
            }

            transaction.FileUpload = file;
            transaction.Status = Enumerations.TransactionStatus.Awaiting_Approval;

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<string>(data: "Saved successfully");
        }

        public async Task<ResultModel<string>> ApproveRejectTransaction(TransactionApprovalVM model)
        {
            var transaction = await _transactionRepo.GetAll().SingleOrDefaultAsync(m => m.Id == model.TransactionId);

            if (transaction == null)
            {
                return new ResultModel<string>(errorMessage: "Transaction was not found.");
            }

            if (model.Approve)
            {
                transaction.Status = Enumerations.TransactionStatus.Paid;
            }
            else
            {
                transaction.Status = Enumerations.TransactionStatus.Rejected;
                transaction.Comment = model.Comment;
            }

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<string>(data: "Saved successfully");
        }

    }
}
