using ArrayToPdf;
using ClosedXML.Excel;
using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.Transaction;
using IPagedList;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.FileStorage;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

            if (check.InvoiceComponents?.Sum(m => m.Amount) < (check.Transactions?.
                                        Where(m=>m.Status != Enumerations.TransactionStatus.Rejected)
                                        .Sum(m => m.Amount) + model.Amount))
            {
                return new ResultModel<string>(errorMessage: "Amount to be paid would be greater than the Amount payable.");
            }

            var transaction = new Transaction()
            {
                InvoiceId = model.InvoiceId,
                Status = Enumerations.TransactionStatus.Pending,
                Amount = model.Amount,
                Description = model.Description,
                PaymentChannel = Enumerations.PaymentChannel.Bank_Deposit
            };

            _transactionRepo.Insert(transaction);

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<string>(data: "Saved successfully");
        }

        public async Task<ResultModel<List<TransactionVM>>> GetAllPendingTransactions(long? studentId)
        {
            var pendingTransactions = _transactionRepo.GetAll()
                        .Where(n => n.Status == Enumerations.TransactionStatus.Pending);
            if (!(studentId is null))
            {
                pendingTransactions = pendingTransactions.Where(n => n.Invoice.StudentId == studentId);
            }

            return new ResultModel<List<TransactionVM>>
                (
                    data: await pendingTransactions.Select(m => new TransactionVM()
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
            var query = await _transactionRepo.GetAll().OrderByDescending(m=>m.Id)
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
                            StudentRegNumber = m.Invoice.Student.RegNumber,
                            TotalAmount = m.Invoice.InvoiceComponents.Where(m=>m.IsSelected).Sum(n=>n.Amount),
                            TotalPaid = m.Invoice.Transactions.Where(m=>
                                m.Status == Enumerations.TransactionStatus.Awaiting_Approval || 
                                m.Status == Enumerations.TransactionStatus.Paid).Sum(n=>n.Amount),
                        }
                    )
                    .ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            return new ResultModel<PaginatedModel<TransactionVM>>
                {
                    Data = new PaginatedModel<TransactionVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
                };
        }

        public async Task<ResultModel<TransactionDetailsVM>> GetTransaction(long transactionId)
        {
            return new ResultModel<TransactionDetailsVM>
                (
                    data: await _transactionRepo.GetAll()
                        .Where(n => n.Id == transactionId)
                        .Select(m => new TransactionDetailsVM()
                        {
                            Description = m.Description,
                            Amount = m.Amount,
                            InvoiceId = m.InvoiceId,
                            status = m.Status,
                            TransactionId = m.Id,
                            DueDate = m.Invoice.PaymentDate,
                            FeeType = m.Invoice.Fee.Name,
                            FileId = m.FileUploadId.ToString(),
                            StudentRegNumber = m.Invoice.Student.RegNumber,
                            PaymentDescription = m.PaymentDescription,
                            PaymentReference = m.PaymentReference,
                            channel = m.PaymentChannel,
                        }
                    ).FirstOrDefaultAsync()
                );
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
                            FileId = m.FileUploadId.ToString(),
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
            transaction.PaymentDescription = model.PaymentDescription;
            transaction.PaymentReference = model.PaymentReference;

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


        public async Task<ResultModel<List<TransactionVM>>> GetAllTransactionReportByStatus(TransStatus model)
        {
            var resultModel =  new ResultModel<List<TransactionVM>>();

            var query = await _transactionRepo.GetAll().Where(x => x.Status == model.Status).OrderByDescending(m => m.Id)
                .Select(m => new TransactionVM()
                {
                    Description = m.Description,
                    Amount = m.Amount,
                    InvoiceId = m.InvoiceId,
                    status = m.Status,
                    TransactionId = m.Id,
                    DueDate = m.Invoice.PaymentDate,
                    FeeType = m.Invoice.Fee.Name,
                    StudentRegNumber = m.Invoice.Student.RegNumber,
                    TotalAmount = m.Invoice.InvoiceComponents.Where(m => m.IsSelected).Sum(n => n.Amount)                    
                }).ToListAsync();

            resultModel = new ResultModel<List<TransactionVM>>(query);

            return resultModel;
        }

        public async Task<ResultModel<ExportPayloadVM>> ExportTransactionRecordExcel(List<TransactionVM> model)
        {
            var resultModel = new ResultModel<ExportPayloadVM>();
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var workSheet = workbook.Worksheets.Add("TeacherSheet");

                    for (int i = 1; i <= 10; i++)
                    {
                        var headFormat = workSheet.Cell(1, i);
                        headFormat.Style.Font.SetBold();
                        headFormat.WorksheetRow().Height = 10;
                    }

                    var currentRow = 1;

                    workSheet.Cell(1, 1).Value = "InvoiceId";
                    workSheet.Cell(1, 2).Value = "Amount";
                    workSheet.Cell(1, 3).Value = "status";
                    workSheet.Cell(1, 4).Value = "DueDate";
                    workSheet.Cell(1, 5).Value = "Description";
                    workSheet.Cell(1, 6).Value = "FeeType";
                    workSheet.Cell(1, 7).Value = "StudentRegNumber";
                    workSheet.Cell(1, 8).Value = "TotalAmount";



                    foreach (var data in model)
                    {
                        currentRow += 1;
                        workSheet.Cell(currentRow, 1).Value = $"{data.InvoiceId}";
                        workSheet.Cell(currentRow, 2).Value = $"{data.Amount}";
                        workSheet.Cell(currentRow, 3).Value = $"{data.status}";
                        workSheet.Cell(currentRow, 4).Value = $"{data.DueDate}";
                        workSheet.Cell(currentRow, 5).Value = $"{data.Description}";
                        workSheet.Cell(currentRow, 6).Value = $"{data.FeeType}";
                        workSheet.Cell(currentRow, 7).Value = $"{data.StudentRegNumber}";
                        workSheet.Cell(currentRow, 8).Value = $"{data.TotalAmount}";

                    }
                    var byteData = new byte[0];
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        byteData = content;
                    }

                    var payload = new ExportPayloadVM
                    {
                        FileName = "INVOICE-RECORD",
                        Base64String = Convert.ToBase64String(byteData)
                    };

                    resultModel.Data = payload;
                    resultModel.TotalCount = model.Count;
                }
            }
            catch (Exception ex)
            {
                resultModel.AddError($"Exception Occured : {ex.Message}");
                return resultModel;
            }


            return resultModel;
        }

        public async Task<ResultModel<ExportPayloadVM>> ExportTransactionRecordPDF(List<TransactionVM> model)
        {
            var resultModel = new ResultModel<ExportPayloadVM>();

            var table = new DataTable("AttendanceReport");

            table.Columns.Add("INVOICE-ID", typeof(long));
            table.Columns.Add("AMOUNT", typeof(decimal));
            table.Columns.Add("STATUS", typeof(string));
            table.Columns.Add("DESCRIPTION", typeof(string));
            table.Columns.Add("DUE-DATE", typeof(string));
            table.Columns.Add("FEE-TYPE", typeof(string));
            table.Columns.Add("STUDENTREGNUM", typeof(string));
            table.Columns.Add("TOTAL-AMOUTN", typeof(decimal));

            foreach (var item in model)
            {
                table.Rows.Add(item.InvoiceId, item.Amount, item.Status, item.Description,
                    $"{item.DueDate:yyyy:MMM:dd}",item.FeeType,item.StudentRegNumber, item.TotalAmount);
            }

            var pdf = table.ToPdf();

            var payload = new ExportPayloadVM
            {
                FileName = "INVOICE-REPORT",
                Base64String = Convert.ToBase64String(pdf)
            };

            resultModel.Data = payload;


            return resultModel;
        }

        public async Task<ResultModel<PaginatedModel<TransactionVM>>> ViewAllTransactionReportByStatus(TransStatus model, QueryModel queryModel)
        {
            var query = await _transactionRepo.GetAll()
                .Where(x => x.Status == model.Status)
                .OrderByDescending(m => m.Id)
                .Select(m => new TransactionVM()
                {
                    Description = m.Description,
                    Amount = m.Amount,
                    InvoiceId = m.InvoiceId,
                    status = m.Status,
                    TransactionId = m.Id,
                    DueDate = m.Invoice.PaymentDate,
                    FeeType = m.Invoice.Fee.Name,
                    StudentRegNumber = m.Invoice.Student.RegNumber,
                    TotalAmount = m.Invoice.InvoiceComponents.Where(m => m.IsSelected).Sum(n => n.Amount)
                }).ToPagedListAsync(queryModel.PageIndex, queryModel.PageSize);

            query = query.Where(x => x.DueDate >= model.From && x.DueDate <= model.To)
                .ToPagedList(queryModel.PageIndex, queryModel.PageSize);

            return new ResultModel<PaginatedModel<TransactionVM>>
            {
                Data = new PaginatedModel<TransactionVM>(query, queryModel.PageIndex, queryModel.PageSize, query.TotalItemCount)
            };
        }

    }
}
