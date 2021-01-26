using Auth.Core.ViewModels.Parent;
using Auth.Core.ViewModels.School;
using Auth.Core.ViewModels.Student;
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
        Task<ResultModel<PaginatedModel<ParentListVM>>> GetAllParents(QueryModel vm);
        Task<ResultModel<PaginatedModel<ParentListVM>>> GetAllParentsInSchool(QueryModel vm);
        Task<ResultModel<ParentDetailVM>> GetParentsForStudent(long studId);
        Task<ResultModel<ParentDetailVM>> GetParentById(long Id);
        Task<ResultModel<List<SchoolParentViewModel>>> GetStudentsSchools(long currentUserId);
        Task<ResultModel<List<StudentParentVM>>> GetStudentsInSchool(long parentId);
        Task<ResultModel<ParentDetailVM>> AddNewParent(AddParentVM vm);
        Task<ResultModel<ParentDetailVM>> UpdateParent(long Id,UpdateParentVM vm);
        Task<ResultModel<string>> DeleteParent(long Id);


    }
}
