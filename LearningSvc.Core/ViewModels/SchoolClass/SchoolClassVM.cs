using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.SchoolClass
{
    public class SchoolClassVM
    {

        public long Id { get; set; }
        public string Name { get; set; }

        public static implicit operator SchoolClassVM(Models.SchoolClass model)
        {
            return model == null ? null : new SchoolClassVM
            {
                Id = model.Id,
                Name = model.Name,

            };
        }
    }
}
