using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Role
{
    public class AddRoleVM
    {
        public int? TenantId { get; set; }
        public string RoleName { get; internal set; }
    }
}
