using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UserManagement.Core.Models;

namespace UserManagement.Core.ViewModels
{
   public class SchoolUpdateVM
    {   
        [Required]
        public long Id { get; set; }
        public string Name { get; set; }

        public static implicit operator SchoolUpdateVM(School model)
        {
            return model == null ? null : new SchoolUpdateVM
            {
                Id = model.Id,
                Name = model.Name
            };
        }

    }
}
