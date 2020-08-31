using Auth.Core.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.SchoolClass
{
   public class ListClassVM
    {
        public int Id { get; internal set; }
        public string Name { get; set; }
        public List<StudentVM> StudentVMs { get; set; }

        public static implicit operator ListClassVM(Models.SchoolClass model)
        {
            return model == null ? null : new ListClassVM
            {
                StudentVMs = (List<StudentVM>)model.Students
            };
        }
    }
}
