using Auth.Core.Models.Contact;
using Auth.Core.Models.Users;
using Shared.Entities;
using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models
{
    public class SchoolGroup : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string WebsiteAddress { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public bool IsActive { get; set; }
        public List<SchoolContactDetails> SchoolContactDetails { get; set; } = new List<SchoolContactDetails>();
        public List<FileUpload> FileUploads { get; set; } = new List<FileUpload>();    

        public List<School> Schools { get; set; }
    }
}