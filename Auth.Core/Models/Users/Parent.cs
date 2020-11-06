using Shared.Entities;
using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Users
{
    public class Parent : FullAuditedEntity<long>
    {
        public string Address { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
        public List<Student> Students { get; set; }
    }
}
