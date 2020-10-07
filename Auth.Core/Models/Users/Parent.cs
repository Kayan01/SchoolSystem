using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Users
{
    public class Parent : Person
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Student> Students { get; set; }
    }
}
