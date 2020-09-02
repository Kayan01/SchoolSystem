using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Contacts
{
    public class SchoolContactDetails : ContactDetails
    {
        public long SchoolId { get; set; }
        public School School { get; set; }
    }
}
