using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.RoleModels
{
    public class AddUserToRolesVM
    {
        public long UserId { get; set; }
        public List<long> RoleIds { get; set; }
    }

    public class RemoveUserFromRoleVM
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
    }

    public class AddPermissionsToRoleVM
    {
        public long RoleId { get; set; }
        public List<int> PermissionIds { get; set; } = new List<int>();
    }

    public class RemovePermissionsFromRoleVM
    {
        public long RoleId { get; set; }
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
}
