using FinanceSvc.Core.ViewModels.Invoice;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<ResultModel<PaginatedModel<InvoicePaymentVM>>> GetPaymentInvoices(long? sessionId, int? termSequence, QueryModel queryModel);
        Task<ResultModel<PaginatedModel<InvoicePaymentHistoryVM>>> GetPaymentHistoryInvoices(long? sessionId, int? termSequence, QueryModel queryModel);
        Task<ResultModel<PaginatedModel<InvoiceVM>>> GetAllInvoices(long? sessionId, int? termSequence, QueryModel queryModel);
        Task<ResultModel<PaginatedModel<InvoicePaymentVM>>> GetInvoices(InvoiceRequestVM model, QueryModel queryModel);
        Task<ResultModel<PaginatedModel<InvoicePendingPaymentVM>>> GetPendingPaymentInvoices(long? sessionId, int? termSequence, QueryModel queryModel);
        Task<ResultModel<InvoiceDetailVM>> GetInvoice(long id);
        Task<ResultModel<string>> AddInvoice(InvoicePostVM model);
        Task<ResultModel<string>> UpdateInvoiceComponentSelection(InvoiceComponentSelectionUpdateVM vm);
    }
}
