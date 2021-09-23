using Auth.Core.Enumerations;
using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models
{
    public class SchoolSubscription : FullAuditedEntity<long>
    {

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PricePerStudent { get; set; }
        public int ExpectedNumberOfStudent { get; set; }

        public long SchoolId { get; set; }
        public School School { get; set; }

    }
}
