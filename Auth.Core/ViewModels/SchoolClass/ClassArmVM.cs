using Auth.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.SchoolClass
{

    public class AddClassArm
    {
        public string Name { get; set; }
        public bool Status { get; set; }
    }
    public class UpdateClassArmVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
    public  class ClassArmVM
    {
        public long Id { get; internal set; }

        public string Name { get; set; }
        public bool Status { get; set; }
        public static explicit operator ClassArmVM(ClassArm model)
        {
            return model == null ? null : new ClassArmVM
            {
                Id = model.Id,
                Name = model.Name,
                Status = model.IsActive
            };
        }
    }
}
