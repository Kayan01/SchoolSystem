using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.Subject
{
    public class SubjectVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator SubjectVM(Models.Subject model)
        {
            return model == null ? null : new SubjectVM
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = model.IsActive,
            };
        }
    }
}
