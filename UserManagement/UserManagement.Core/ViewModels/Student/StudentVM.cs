using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Core.Models;

namespace UserManagement.Core.ViewModels.Student
{
    public class StudentVM
    {
        public long Id { get; internal set; }
        public string FirstName { get;  set; }
        public string LastName { get;  set; }

        public static implicit operator StudentVM(Models.Student model)
        {
            return model == null ? null : new StudentVM
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
        }
    }
}
