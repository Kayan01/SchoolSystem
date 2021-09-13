using Auth.Core.ViewModels.Promotion;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces
{
    public interface IPromotionService
    {
        public Task PromoteAllStudent(PromotionSharedModel model);
        Task<ResultModel<PaginatedModel<ClassPoolVM>>> GetClassPool(QueryModel vm, long sessionId, long? fromClassId);
        Task<ResultModel<string>> PostClassPool(List<ClassPoolVM> VMs);
        Task<ResultModel<PaginatedModel<ClassPoolVM>>> GetWithdrawnList(QueryModel vm, long sessionId, long? fromClassId);
        Task<ResultModel<PaginatedModel<ClassPoolVM>>> GetRepeatList(QueryModel vm, long sessionId, long? fromClassId);
        Task<ResultModel<List<PromotionHighlightVM>>> GetPromotionHighlight(long sessionId, long? fromClassId);
    }
}
