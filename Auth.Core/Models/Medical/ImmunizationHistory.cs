using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Medical
{
    public class ImmunizationHistory: FullAuditedEntity<long>
    {
        public DateTime DateImmunized { get; set; }
        public int Age { get; set; }
        public string Vaccine { get; set; }
    }
}
