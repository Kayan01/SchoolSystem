using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Core.Models
{
    public interface ITenantModelType 
    {
        public long SchoolId { get; set; }
    }
}
