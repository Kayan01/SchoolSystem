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
        public UserType UserType { get; set; } = UserType.Admin;
        public User User { get; set; }
    }
}
