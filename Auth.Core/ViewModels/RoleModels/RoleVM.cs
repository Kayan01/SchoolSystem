using Auth.Core.Models;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.RoleModels
{
    public class RoleVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int usersCount { get;  set; }

        public static implicit operator RoleVM(SchoolTrackRole model)
        {
            return model == null ? null : new RoleVM
            {
                Id = model.Id,
                Name = model.Name
            };
        }
    }
}
