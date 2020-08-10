using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Core.Models
{
    public abstract class BaseModel : AuditedEntity<long>
    {
        public int SchoolId { get; set; }
    }
}
