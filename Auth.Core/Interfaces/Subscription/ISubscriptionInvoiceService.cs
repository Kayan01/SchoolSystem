using Auth.Core.ViewModels.Subscription;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces
{
    public interface ISubscriptionInvoiceService
    {
        Task<ResultModel<SubcriptionInvoiceVM>> GetNextSubsciptionInvoice(long schoolId);
        Task<ResultModel<string>> PostNextSubsciptionInvoice(SubcriptionInvoiceVM invoice);
        Task<ResultModel<SubcriptionInvoiceVM>> GetArrearsSubsciptionInvoice(long schoolId);
        Task<ResultModel<string>> PostArrearsSubsciptionInvoice(SubcriptionInvoiceVM invoice);
        Task<ResultModel<List<GetSubcriptionInvoiceVM>>> GetUnpaidSubsciptionInvoice(long schoolId);
        Task<ResultModel<string>> MarkInvoiceAsPaid(PayInvoiceVM vm);
    }
}
