using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels
{
    public class UserListVM
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public static implicit operator UserListVM(User model)
        {
            return model == null ? null : new UserListVM
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };
        }
    }
}
