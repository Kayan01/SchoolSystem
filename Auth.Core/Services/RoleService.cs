using Auth.Core.Interfaces;
using Auth.Core.ViewModels;
using Shared.Extensions;
using Auth.Core.ViewModels.RoleModels;
using Microsoft.AspNetCore.Identity;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.Entities;
using Shared.Permissions;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Shared.Utils;
using Shared.Pagination;
using Auth.Core.Models;
using Shared.DataAccess.Repository;
using Shared.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace Auth.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IRepository<SchoolTrackRole, long> _schoolRoleRepo;
        private readonly ITenantResolutionStrategy _tenantResolutionStrategy;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(UserManager<User> userManager, RoleManager<Role> roleManager,
            IRepository<SchoolTrackRole, long> schoolRoleRepo,
            ITenantResolutionStrategy tenantResolutionStrategy,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _schoolRoleRepo = schoolRoleRepo;
            _tenantResolutionStrategy = tenantResolutionStrategy;
        }

        public async Task<ResultModel<IEnumerable<PermissionVM>>> GetAllPermissions()
        {
            var permissions = (Enum.GetValues(typeof(Permission)) as Permission[])
                                .Select(x => new PermissionVM
                                {
                                    Id = (int)x,
                                    Name = x.ToString(),
                                    Description = x.GetDescription()
                                });

            return new ResultModel<IEnumerable<PermissionVM>>(permissions, "Success");
        }

        public async Task<ResultModel<IEnumerable<PermissionVM>>> GetRolePermissions(long roleId)
        {
            var schRole = _schoolRoleRepo.FirstOrDefault(roleId);
            if (schRole == null)
                return new ResultModel<IEnumerable<PermissionVM>>("School Role not found");

            var role = await _roleManager.FindByNameAsync(schRole.RoleName);
            if (role == null)
                return new ResultModel<IEnumerable<PermissionVM>>("Role not found");

            var permissions = (await _roleManager.GetClaimsAsync(role))
                                .Where(x => Enum.IsDefined(typeof(Permission), x.Value))
                                    .Select(x => Enum.Parse<Permission>(x.Value));

            return new ResultModel<IEnumerable<PermissionVM>>(permissions.Select(x => (PermissionVM)x), "Success");
        }

        public async Task<ResultModel<PaginatedModel<RoleVM>>> GetRoles(QueryModel model)
        {
            var query = _schoolRoleRepo.GetAll();
            var pagedData = await PaginatedList<SchoolTrackRole>.CreateAsync(query, model.PageIndex, model.PageSize);

            return new ResultModel<PaginatedModel<RoleVM>>
                (new PaginatedModel<RoleVM>(pagedData.Select(x => (RoleVM)x), model.PageIndex, model.PageSize, pagedData.TotalCount));
        }

        public async Task<ResultModel<IEnumerable<string>>> GetUserRoles(long userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new ResultModel<IEnumerable<string>>("User not found");

            var roles = (await _userManager.GetRolesAsync(user)).Select(x => x.Split('|')?[0]);

            return new ResultModel<IEnumerable<string>>(roles, "Success");
        }

        public async Task<ResultModel<IEnumerable<UserListVM>>> GetUsersInRole(RoleRequestModel model)
        {
            var schRole = _schoolRoleRepo.FirstOrDefault(model.RoleId);
            if (schRole == null)
                return new ResultModel<IEnumerable<UserListVM>>("School Role not found");

            var users = await _userManager.GetUsersInRoleAsync(schRole.RoleName);
            //TODO filter users by TenantId, this is currently not doable as users are not tied to a Tenant
            //hence this endpoint can only be available to super admin

            return new ResultModel<IEnumerable<UserListVM>>(users.Select(x => (UserListVM)x), "Success");
        }

        public async Task<ResultModel<RoleVM>> CreateRole(CreateRoleVM model)
        {
            if (model.Name.Contains('|'))
                return new ResultModel<RoleVM>("Role name contains invalid character |");

            _unitOfWork.BeginTransaction();
            var schoolRole = _schoolRoleRepo.FirstOrDefault(x => x.Name == model.Name.ToUpper());

            if (schoolRole != null)
                return new ResultModel<RoleVM>("Role already exists in the school");

            schoolRole = new SchoolTrackRole { Name = model.Name.ToUpper() };
            schoolRole = _schoolRoleRepo.Insert(schoolRole);

            var role = await _roleManager.FindByNameAsync(schoolRole.RoleName);

            if (role != null)
                return new ResultModel<RoleVM>("Role already exists");

            role = new Role
            {
                Name = schoolRole.RoleName,
                IsActive = true
            };

            var createResult = await _roleManager.CreateAsync(role);

            if (!createResult.Succeeded)
                return new ResultModel<RoleVM>(createResult.Errors.ToString());

            await _unitOfWork.SaveChangesAsync();

            var permissions = model.PermissionIds.Select(x => (Permission)x);

            var addPermissionResult = await AddPermissionToRole(role, model.PermissionIds);
            if (addPermissionResult.HasError)
                return new ResultModel<RoleVM>(addPermissionResult.ErrorMessages.FirstOrDefault());

            _unitOfWork.Commit();

            return new ResultModel<RoleVM>(schoolRole, "Success");
        }

        public async Task<ResultModel<RoleVM>> AddPermissionsToRole(AddPermissionsToRoleVM model)
        {
            var schRole = _schoolRoleRepo.FirstOrDefault(model.RoleId);
            if (schRole == null)
                return new ResultModel<RoleVM> ("School Role not found");

            var role = await _roleManager.FindByNameAsync(schRole.RoleName);
            if (role == null)
                return new ResultModel<RoleVM> ("Role not found");

            var existingPermissions = (await _roleManager.GetClaimsAsync(role))
                                .Where(x => Enum.IsDefined(typeof(Permission), x.Value))
                                    .Select(x => (int)Enum.Parse<Permission>(x.Value));

            var addPermissionResult = await AddPermissionToRole(role, model.PermissionIds.Except(existingPermissions).ToList());
            if (addPermissionResult.HasError)
                return new ResultModel<RoleVM>(addPermissionResult.ErrorMessages.FirstOrDefault());

            return new ResultModel<RoleVM>(schRole);
        }

        public async Task<ResultModel<List<RoleVM>>> AddUserToRoles(AddUserToRolesVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
                return new ResultModel<List<RoleVM>> ("User not found");

            var schRoles = _schoolRoleRepo.GetAll().Where(x=> model.RoleIds.Contains(x.Id)).ToList();

            if (schRoles.Count < 1)
                return new ResultModel<List<RoleVM>>("School Roles not found");

            //Remove existing role if any
            var tenantId = _tenantResolutionStrategy.GetTenantIdentifier().ToString();
            var userRoles = await _userManager.GetRolesAsync(user);
            var removeRoleResult = await _userManager.RemoveFromRolesAsync(user, userRoles);


            var rolenames = schRoles.Select(x => x.RoleName).ToList();

            var addRoleResult = await _userManager.AddToRolesAsync(user, rolenames);
            if (!addRoleResult.Succeeded)
                return new ResultModel<List<RoleVM>>($"failed to add role, {string.Join("; ", addRoleResult.Errors.Select(x => x.Description))}");

            return new ResultModel<List<RoleVM>>(schRoles.Select(x=> (RoleVM)x).ToList(), "Success");
        }

        public async Task<ResultModel<RoleVM>> RemovePermissionsFromRole(RemovePermissionsFromRoleVM model)
        {
            var schRole = _schoolRoleRepo.FirstOrDefault(model.RoleId);
            if (schRole == null)
                return new ResultModel<RoleVM>("School Role not found");

            var role = await _roleManager.FindByNameAsync(schRole.RoleName);
            if (role == null)
                return new ResultModel<RoleVM>("Role not found");

            var permissions = model.PermissionIds.Select(x => (Permission)x);

            foreach (var permission in permissions)
            {
                var removePermissionResult = await _roleManager.RemoveClaimAsync(role, new Claim(nameof(Permission), permission.ToString()));
                if (!removePermissionResult.Succeeded)
                {
                    await _roleManager.DeleteAsync(role);
                    return new ResultModel<RoleVM>(removePermissionResult.Errors.FirstOrDefault().Description);
                }
            }

            return new ResultModel<RoleVM>(schRole);
        }

        public async Task<ResultModel<RoleVM>> RemoveUserFromRole(RemoveUserFromRoleVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
                return new ResultModel<RoleVM>("User not found");

            var schRole = _schoolRoleRepo.FirstOrDefault(model.RoleId);
            if (schRole == null)
                return new ResultModel<RoleVM>("School Role not found");

            var addRoleResult = await _userManager.RemoveFromRoleAsync(user, schRole.RoleName);
            if (!addRoleResult.Succeeded)
                return new ResultModel<RoleVM>($"failed to remove role {schRole.Name} to user {user.FullName}");

            return new ResultModel<RoleVM>(schRole, "Success");
        }

        private async Task<ResultModel<Role>> AddPermissionToRole(Role role, List<int> permissionIds)
        {
            var permissions = permissionIds.Where(x => Enum.IsDefined(typeof(Permission), x)).Select(x => (Permission)x);

            foreach (var permission in permissions)
            {
                var addRoleResult = await _roleManager.AddClaimAsync(role, new Claim(nameof(Permission), permission.ToString()));
                if (!addRoleResult.Succeeded)
                {
                    await _roleManager.DeleteAsync(role);
                    return new ResultModel<Role>(addRoleResult.Errors.FirstOrDefault().Description);
                }
            }

            return new ResultModel<Role>(role, "Success");
        }

        public async Task<ResultModel<bool>> DeleteRole(long Id)
        {
            var schRole = await _schoolRoleRepo.GetAll()
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();

            if (schRole != null)
            {
                return new ResultModel<bool>($"No role exists with id : {Id}");
            }

            //get permissions
           var permissionResult = await GetRolePermissions(Id);

            if (permissionResult.HasError)
            {
                return new ResultModel<bool>(permissionResult.ErrorMessages);
            }
            //remove permissions
            var removePermissionsResult = await RemovePermissionsFromRole(
                new RemovePermissionsFromRoleVM
                {
                    PermissionIds = permissionResult.Data.Select(x => x.Id).ToList(),
                    RoleId = Id
                });

            if (removePermissionsResult.HasError)
            {
                return new ResultModel<bool>(removePermissionsResult.ErrorMessages);
            }

            //delete school role
            await _schoolRoleRepo.DeleteAsync(schRole);


            var role = await _roleManager.FindByNameAsync(schRole.RoleName);

            if (role != null)
                return new ResultModel<bool>("Role doesnt exists");


           var deleteResult = await _roleManager.DeleteAsync(role);

            if (!deleteResult.Succeeded)
            {
                return new ResultModel<bool>(deleteResult.Errors.Select(x=> x.Description).ToList());
            }

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<bool>(true, "Deleted role successfully");
        }
    }
}
