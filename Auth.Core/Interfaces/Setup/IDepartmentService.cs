using Auth.Core.ViewModels.Setup;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces.Setup
{
    public interface IDepartmentService
    {
        Task<ResultModel<DepartmentVM>> AddDepartment(AddDepartmentVM model);

        Task<ResultModel<bool>> DeleteDepartment(long Id);

        Task<ResultModel<PaginatedModel<DepartmentListVM>>> GetAllDepartments(QueryModel vm);
        Task<ResultModel<DepartmentVM>> GetDepartmentById(long Id);

        Task<ResultModel<DepartmentVM>> UpdateDepartment(UpdateDepartmentVM model, long id);
    }
}
