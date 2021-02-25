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
        Task<ResultModel<List<InvoicePaymentVM>>> GetPaymentInvoices(string session, string term);
        Task<ResultModel<List<InvoicePaymentHistoryVM>>> GetPaymentHistoryInvoices(string session, string term);
        Task<ResultModel<List<InvoiceVM>>> GetAllInvoices(string session, string term);
        Task<ResultModel<List<InvoicePaymentVM>>> GetInvoices(InvoiceRequestVM model);
        Task<ResultModel<List<InvoicePendingPaymentVM>>> GetPendingPaymentInvoices(string session, string term);
        Task<ResultModel<InvoiceDetailVM>> GetInvoice(long id);
        Task<ResultModel<string>> AddInvoice(InvoicePostVM model);
    }
}
