using Auth.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.SchoolClass
{
    public  class ClassArmVM
    {
        public long Id { get; internal set; }
        public string Name { get; set; }

        public static explicit operator ClassArmVM(ClassArm model)
        {
            return model == null ? null : new ClassArmVM
            {
                Id = model.Id,
                Name = model.Name
            };
        }
    }
}
