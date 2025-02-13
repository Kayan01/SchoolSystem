﻿using Auth.Core.Models;
using Shared.Entities;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Auth.Core
{
    public abstract class Person : FullAuditedEntity<long>, ITenantModelType
    {
        [ForeignKey(nameof(School))]
        public long TenantId { get; set; }
        public long UserId { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; }
        public string RegNumber { get; set; }

        public User User { get; set; } //Test this out

        public School School { get; set; }

        public List<FileUpload> FileUploads { get; set; } = new List<FileUpload>();
    }
}