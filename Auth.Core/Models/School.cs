using Auth.Core.Models.Contact;
using Auth.Core.Models.Users;
using Shared.Entities;
using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models
{
    public class School : AuditedEntity<long>
    {
        public string Name { get; set; }
        public string DomainName { get; set; }
        public string WebsiteAddress { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public string ClientCode { get; set; }
        public List<SchoolContactDetails> SchoolContactDetails { get; set; }
        public List<FileUpload> FileUploads { get; set; } = new List<FileUpload>();

        public List<Student> Students { get; set; }
        public List<Staff> Staffs { get; set; }
        public List<TeachingStaff> TeachingStaffs { get; set; }
        public List<SchoolSection> SchoolSections { get; set; }
    }
}