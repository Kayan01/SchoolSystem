using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.UserDetails
{
    public class WorkExperience: FullAuditedEntity<long>
    {
        public string WorkRole { get; set; }
        public string WorkCompanyName { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}
