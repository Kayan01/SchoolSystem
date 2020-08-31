using Auth.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.SchoolClass
{
    public  class ClassGroupVM
    {
        public long Id { get; internal set; }
        public string Name { get; set; }

        public static explicit operator ClassGroupVM(ClassGroup model)
        {
            return model == null ? null : new ClassGroupVM
            {
                Id = model.Id,
                Name = model.Name
            };
        }
    }
}
