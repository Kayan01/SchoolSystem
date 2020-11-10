using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Student
{
    public class CreateStudentVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { internal get; set; }
    }
}
