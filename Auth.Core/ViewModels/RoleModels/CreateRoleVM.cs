using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.RoleModels
{
    public class CreateRoleVM
    {
        public string Name { get; set; }
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
}
