using Shared.Entities;
using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Users
{
    public class Parent : FullAuditedEntity<long>
    {
        public string Title { get; set; }
        public string RegNumber { get; set; }
        public string Sex { get; set; }
        public string Occupation { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public string SecondaryEmail { get; set; }
        public string HomeAddress { get; set; }
        public string OfficeAddress { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public  bool Status { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public List<Student> Students { get; set; }
        public List<FileUpload> FileUploads { get; set; }
    }
}
