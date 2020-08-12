﻿using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Core.Models
{
    public class School : AuditedEntity<long>
    {
        public string Name { get; set; }
        public string LogoPath { get; set; }
    }
}
