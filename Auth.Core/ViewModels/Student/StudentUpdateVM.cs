using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Student
{
    public class StudentUpdateVM
    {
        public string FirstName { get;  set; }
        public long UserId { get;  set; }
        public string LastName { get; set; }
        public long? ClassId { get; internal set; }
        public string PhoneNumber { get; internal set; }
    }
}
