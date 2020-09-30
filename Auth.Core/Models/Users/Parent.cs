using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Users
{
    public class Parent : Person
    {
        public long StudentId { get; set; }
        public Student Student { get; set; }
    }
}
