using Shared.Extensions;
using Shared.Permissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.RoleModels
{
    public class PermissionVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static implicit operator PermissionVM(Permission model)
        {
            return new PermissionVM
            {
                Id = (int)model,
                Name = model.ToString(),
                Description = model.GetDescription(),
            };
        }
    }
}
