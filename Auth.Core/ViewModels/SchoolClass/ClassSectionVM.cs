using Auth.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.SchoolClass
{
    public class ClassSectionVM
    {
        public long Id { get; internal set; }
        public string  Name { get; set; }

        public static explicit operator ClassSectionVM(SchoolSection model)
        {
            return model == null ? null : new ClassSectionVM
            {
                Id = model.Id,
                Name = model.Name
            };
        }
    }
}
