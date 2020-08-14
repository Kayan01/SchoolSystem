using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Core.ViewModels.Student;

namespace UserManagement.Core.ViewModels.SchoolClass
{
    public class ClassVM
    {
        public long Id { get; internal set; }
        public string  Name { get; set; }
        public List<StudentVM> StudentVMs { get; set; }

        public static implicit operator ClassVM(Models.SchoolClass model)
        {
            return model == null ? null : new ClassVM
            {
                Id = model.Id,
                Name = model.Name,
                StudentVMs = (List<StudentVM>)model.Students
            };
        }
    }


}
