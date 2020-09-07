using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels
{
    public class AuthUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}