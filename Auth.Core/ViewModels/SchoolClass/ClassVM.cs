using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Auth.Core.ViewModels.Student;

namespace Auth.Core.ViewModels.SchoolClass
{
    public class ClassVM
    {
        public long Id { get; internal set; }
        [Required]
        public string  Name { get; set; }
        [Required]
        public long SectionId { get; set; }
        [Required]
        public long ClassGroupId { get; set; }

        public static implicit operator ClassVM(Models.SchoolClass model)
        {
            return model == null ? null : new ClassVM
            {
                Id = model.Id,
                Name = model.Name,
                SectionId = model.SchoolSectionId,
               
            };
        }
    }


}
