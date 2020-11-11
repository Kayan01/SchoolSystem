using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.UserDetails
{
    public class NextOfKin:FullAuditedEntity<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string OtherName { get; set; }

        public string Relationship { get; set; }
        public string Occupation { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Town { get; set; }
    }
}
