using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Users
{
    public class Parent : Person
    {
        public List<Student> Students { get; set; }
    }
}
