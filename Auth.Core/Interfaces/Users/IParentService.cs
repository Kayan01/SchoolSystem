using Auth.Core.ViewModels.Parent;
using IPagedList;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces.Users
{
    public interface IParentService
    {
        Task<ResultModel<PaginatedModel<ParentVM>>> GetAllParents(QueryModel vm);
        Task<ResultModel<List<ParentVM>>> GetParentsForStudent(long studId);
        Task<ResultModel<ParentVM>> GetParentById(long Id);

        Task<ResultModel<ParentVM>> AddNewParent(AddParentVM vm);
        Task<ResultModel<ParentVM>> UpdateParent(long Id,UpdateParentVM vm);
        Task<ResultModel<string>> DeleteParent(long Id);


    }
}
