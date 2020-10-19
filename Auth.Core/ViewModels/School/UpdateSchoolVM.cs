using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Auth.Core.Models;

namespace Auth.Core.ViewModels
{
   public class UpdateSchoolVM
    {   
        [Required]
        public long Id { get; set; }
        public string Name { get; set; }

        public static implicit operator UpdateSchoolVM(Models.School model)
        {
            return model == null ? null : new UpdateSchoolVM
            {
                Id = model.Id,
                Name = model.Name
            };
        }

    }
}
