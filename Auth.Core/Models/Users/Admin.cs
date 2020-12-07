using Shared.Entities;
using Shared.Entities.Auditing;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Users
{
    public class Admin : FullAuditedEntity<long>
    {

        public UserType UserType { get; set; } = UserType.GlobalAdmin;

        public long UserId { get; set; }
        public User User { get; set; }

        public List<FileUpload> FileUploads { get; set; } = new List<FileUpload>();
    }
}
