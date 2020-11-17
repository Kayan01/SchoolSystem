using Auth.Core.ViewModels;
using Auth.Core.ViewModels.RoleModels;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces
{
    public interface IRoleService
    {
        Task<ResultModel<PaginatedModel<RoleVM>>> GetRoles(QueryModel model);
        Task<ResultModel<IEnumerable<UserListVM>>> GetUsersInRole(RoleRequestModel model);
        Task<ResultModel<IEnumerable<string>>> GetUserRoles(long userId);
        Task<ResultModel<IEnumerable<PermissionVM>>> GetAllPermissions();
        Task<ResultModel<IEnumerable<PermissionVM>>> GetRolePermissions(long roleId);
        Task<ResultModel<RoleVM>> CreateRole(CreateRoleVM model);
        Task<ResultModel<RoleVM>> AddPermissionsToRole(AddPermissionsToRoleVM model);
        Task<ResultModel<List<RoleVM>>> AddUserToRoles(AddUserToRoleVM model);
        Task<ResultModel<RoleVM>> RemovePermissionsFromRole(RemovePermissionsFromRoleVM model);
        Task<ResultModel<RoleVM>> RemoveUserFromRole(RemoveUserFromRoleVM model);
    }
}
