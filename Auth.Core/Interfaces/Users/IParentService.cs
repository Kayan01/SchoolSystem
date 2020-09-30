using Auth.Core.ViewModels.Parent;
using IPagedList;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces.Users
{
    public interface IParentService
    {
        Task<ResultModel<IPagedList<ParentVM>>> GetAllParents(PagingVM vm);
        Task<ResultModel<List<ParentVM>>> GetParentsForStudent(long studId);
        Task<ResultModel<ParentVM>> GetParentById(long Id);

        Task<ResultModel<string>> AddNewParent(AddParentVM vm);
        Task<ResultModel<string>> UpdateParent(long Id,UpdateParentVM vm);
        Task<ResultModel<string>> DeleteParent(long Id);


    }
}
