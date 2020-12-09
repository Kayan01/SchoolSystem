using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.Student
{
    public class StudentVM
    {
        public long Id { get; set; }
        public string RegNumber { get; set; }
        public string Name { get; set; }

        public static implicit operator StudentVM(Models.Student model)
        {
            return model == null ? null : new StudentVM
            {
                Id = model.Id,
                Name = $"{model.LastName} {model.FirstName}",
                RegNumber = model.RegNumber,
            };
        }
    }
}
