using Auth.Core.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auth.Core.ViewModels.SchoolClass
{
   public class ListClassVM
    {
        public long Id { get; internal set; }
        public string ClassGroup { get; set; }
        public string ClassSection { get; set; }
        public string Name { get; set; }
        public List<StudentVM> Students { get; set; }

        public static implicit operator ListClassVM(Models.SchoolClass model)
        {
            return model == null ? null : new ListClassVM
            {
                Name = model.Name,
                Id = model.Id,
                Students =  model.Students.Select(x => (StudentVM)x).ToList()
            };
        }
    }
}
