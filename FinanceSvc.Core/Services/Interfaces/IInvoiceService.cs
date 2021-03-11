using FinanceSvc.Core.ViewModels.Invoice;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<ResultModel<List<InvoicePaymentVM>>> GetPaymentInvoices(long sessionId, int termSequence);
        Task<ResultModel<List<InvoicePaymentHistoryVM>>> GetPaymentHistoryInvoices(long sessionId, int termSequence);
        Task<ResultModel<List<InvoiceVM>>> GetAllInvoices(long sessionId, int termSequence);
        Task<ResultModel<List<InvoicePaymentVM>>> GetInvoices(InvoiceRequestVM model);
        Task<ResultModel<List<InvoicePendingPaymentVM>>> GetPendingPaymentInvoices(long sessionId, int termSequence);
        Task<ResultModel<InvoiceDetailVM>> GetInvoice(long id);
        Task<ResultModel<string>> AddInvoice(InvoicePostVM model);
        Task<ResultModel<string>> UpdateInvoiceComponentSelection(InvoiceComponentSelectionUpdateVM vm);
    }
}
