using Auth.Core.ViewModels.Subscription;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces
{
    public interface ISchoolSubscriptionService
    {
        Task<ResultModel<string>> AddSchoolSubscription(List<AddSubscriptionVM> VMs);
        Task<ResultModel<PaginatedModel<SubscriptionVM>>> GetAllSchoolSubscription(QueryModel model, long? groupId = null);
    }
}
