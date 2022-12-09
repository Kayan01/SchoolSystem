using FinanceSvc.Core.ViewModels.Transaction;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<ResultModel<string>> NewPendingTransaction(TransactionPostVM model);
        Task<ResultModel<List<TransactionVM>>> GetAllPendingTransactions(long? studentId);
        Task<ResultModel<List<TransactionVM>>> GetTransactionHistory(long studentId);
        Task<ResultModel<PaginatedModel<TransactionVM>>> GetAllTransactions(QueryModel queryModel);
        Task<ResultModel<TransactionDetailsVM>> GetTransaction(long transactionId);
        Task<ResultModel<List<TransactionVM>>> GetAllAwaitingApprovalTransactions();
        Task<ResultModel<string>> UploadTransactionReceipt(TransactionReceiptVM model);
        Task<ResultModel<string>> ApproveRejectTransaction(TransactionApprovalVM model);
        Task<ResultModel<List<TransactionVM>>> GetAllTransactionReportByStatus(TransStatus model);
        Task<ResultModel<ExportPayloadVM>> ExportTransactionRecordExcel(List<TransactionVM> model);
        Task<ResultModel<ExportPayloadVM>> ExportTransactionRecordPDF(List<TransactionVM> model);
        Task<ResultModel<PaginatedModel<TransactionVM>>> ViewAllTransactionReportByStatus(TransStatus model, QueryModel queryModel);
    }
}
