using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class SchoolSharedModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DomainName { get; set; }
        public string WebsiteAddress { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string EmailPassword { get; set; }
    }
}
