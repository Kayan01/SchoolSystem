using System;
using System.Collections.Generic;
using System.Text;
using Auth.Core.Models;

namespace Auth.Core.ViewModels.Student
{
    public class StudentVM
    {
        public long Id { get; internal set; }
        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public string Email { get;  set; }
        public string PhoneNumber { get; set; }
        public string Password { internal get; set; }

        public static implicit operator StudentVM(Models.Student model)
        {
            return model == null ? null : new StudentVM
            {
                Id = model.Id
            };
        }
    }
}
