﻿using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Contact
{
    public class ContactDetails : AuditedEntity<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
