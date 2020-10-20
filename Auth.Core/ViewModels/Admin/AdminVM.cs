using Auth.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels
{
    public class AdminVM
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public static implicit operator AdminVM(Admin model)
        {
            return model == null ? null : new AdminVM
            {
                Id = model.Id,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
              
            };
        }
    }

    public class AddAdminVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class UpdateAdminVM
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
