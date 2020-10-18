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
        public List<Admin> Admins { get; set; }
        public List<FileUpload> FileUploads { get; set; } = new List<FileUpload>();
    }
}