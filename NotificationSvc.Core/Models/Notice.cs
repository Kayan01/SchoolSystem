using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSvc.Core.Models
{
    public class Notice : AuditedEntity<long>
    {
        public string Description { get; set; }
    }
}
