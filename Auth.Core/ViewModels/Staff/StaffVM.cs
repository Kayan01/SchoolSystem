using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Auth.Core.ViewModels.Staff
{
    public class StaffVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Id { get;  set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        [Required]
        public string StaffType { get; set; }

        public static implicit operator StaffVM(Models.Staff model)
        {
            return model == null ? null : new StaffVM
            {
                Id = model.Id,
            };
        }
    }
}
