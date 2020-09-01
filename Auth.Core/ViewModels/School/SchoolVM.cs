using System;
using System.Collections.Generic;
using System.Text;
using Auth.Core.Models;

namespace Auth.Core.ViewModels
{
   public class SchoolVM
    {
        public long Id { get; internal set; }
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
