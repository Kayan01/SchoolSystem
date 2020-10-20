using Shared.Entities;
using Shared.Entities.Auditing;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Users
{
    public class Admin : AuditedEntity<long>
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public UserType UserType { get; set; } = UserType.Admin;
        public bool IsPrimaryContact { get; set; }
        public string PhoneNumber { get; set; }
        public User User { get; set; }
    }
}
