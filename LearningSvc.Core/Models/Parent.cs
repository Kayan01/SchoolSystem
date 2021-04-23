using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class Parent : Person
    {
        public string SecondaryPhoneNumber { get; set; }
        public string SecondaryEmail { get; set; }
        public string HomeAddress { get; set; }
        public string OfficeAddress { get; set; }

        public ICollection<Student> Students { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{LastName} {FirstName}";
            }
        }
    }
}
