using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Core.Models;

namespace UserManagement.Core.ViewModels
{
   public class SchoolVM
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public static implicit operator SchoolVM(School model)
        {
            return model == null ? null : new SchoolVM
            {
                Id = model.Id,
                Name = model.Name
            };
        }

    }
}
