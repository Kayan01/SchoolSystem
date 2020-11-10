using Auth.Core.Models.Setup;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Setup
{
    public class AddDepartmentVM
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
   public class DepartmentVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public static implicit operator DepartmentVM(Department model)
        {
            return model == null ? null : new DepartmentVM
            {
                Id = model.Id,
                Name = model.Name,
                 Status= model.IsActive,
            };
        }
    }
    public class DepartmentListVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public static implicit operator DepartmentListVM(Department model)
        {
            return model == null ? null : new DepartmentListVM
            {
                Id = model.Id,
                Name = model.Name,
                Status = model.IsActive
            };

        }
    }
    public class UpdateDepartmentVM
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

} 
