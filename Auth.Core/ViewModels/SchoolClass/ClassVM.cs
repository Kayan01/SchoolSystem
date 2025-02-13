﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auth.Core.ViewModels.SchoolClass
{
    public class ClassVM
    {
        public long Id { get; internal set; }
        public string  Name { get; set; }
        public string ClassName { get; set; }
        public string ClassArm { get; set; }
        public long SectionId { get; set; }
        public long ClassGroupId { get; set; }

        public static implicit operator ClassVM(Models.SchoolClass model)
        {
            return model == null ? null : new ClassVM
            {
                Id = model.Id,
                ClassName = model.Name,
                ClassArm = model.ClassArm,
                Name = $"{model.Name} {model.ClassArm}",
                SectionId = model.SchoolSectionId,
            };
        }
    }

    public class AddClassVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public long SectionId { get; set; }
        [Required]
        public int Sequence { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public List<long> ClassArmIds { get; set; }
        [Required]
        public bool IsTerminalClass { get; set; }
    }


}
