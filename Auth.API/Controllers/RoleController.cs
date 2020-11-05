using Auth.Core.Interfaces;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.RoleModels;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> Test([FromQuery]QueryModel model)
        {
            throw new Exception("Testing Shit");
        }

        //TODO Delete this Endpoint, It is for Teseting purpose
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> GetClaims([FromQuery]QueryModel model)
        {
            try
            {
                var claims = CurrentUser.Claims.ToList();
                var result = new
                {
                    Permissions = claims.Where(x => x.Type == "Permission").Select(x => x.Value).ToList(),
                    Others = claims.Where(x => x.Type != "Permission").Select(x => new KeyValuePair<string, string>(x.Type, x.Value)).ToList(),
                };
                return ApiResponse<object>(message: "Ok", codes: ApiResponseCodes.OK, data: result );
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RoleVM>>), 200)]
        public async Task<IActionResult> GetRoles([FromQuery]QueryModel model)
        {
            try
            {
                var result = await _roleService.GetRoles(model);
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<IEnumerable<RoleVM>>(message: result.Message, codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserListVM>>), 200)]
        public async Task<IActionResult> GetUsersInRole([FromQuery]RoleRequestModel model)
        {
            try
            {
                var result = await _roleService.GetUsersInRole(model);
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<IEnumerable<UserListVM>>(message: result.Message, codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), 200)]
        public async Task<IActionResult> GetUserRoles(long userId)
        {
            try
            {
                var result = await _roleService.GetUserRoles(userId);
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<IEnumerable<string>>(message: result.Message, codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet()]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PermissionVM>>), 200)]
        public async Task<IActionResult> GetAllPermissions()
        {
            try
            {
                var result = await _roleService.GetAllPermissions();
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<IEnumerable<PermissionVM>>(message: result.Message, codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{roleId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PermissionVM>>), 200)]
        public async Task<IActionResult> GetRolePermissions(long roleId)
        {
            try
            {
                var result = await _roleService.GetRolePermissions(roleId);
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<IEnumerable<PermissionVM>>(message: result.Message, codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RoleVM>), 200)]
        public async Task<IActionResult> CreateRole(CreateRoleVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<List<string>>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _roleService.CreateRole(model);
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<RoleVM>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RoleVM>), 200)]
        public async Task<IActionResult> AddPermissionsToRole(AddPermissionsToRoleVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<List<string>>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _roleService.AddPermissionsToRole(model);
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<RoleVM>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RoleVM>), 200)]
        public async Task<IActionResult> AddUserToRole(AddUserToRoleVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<List<string>>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _roleService.AddUserToRole(model);
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<RoleVM>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RoleVM>), 200)]
        public async Task<IActionResult> RemovePermissionsFromRole(RemovePermissionsFromRoleVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<List<string>>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _roleService.RemovePermissionsFromRole(model);
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<RoleVM>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RoleVM>), 200)]
        public async Task<IActionResult> RemoveUserFromRole(RemoveUserFromRoleVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<List<string>>(ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _roleService.RemoveUserFromRole(model);
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<RoleVM>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}