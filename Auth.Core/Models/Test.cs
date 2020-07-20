using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models
{
    public class Test : FullAuditedEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
