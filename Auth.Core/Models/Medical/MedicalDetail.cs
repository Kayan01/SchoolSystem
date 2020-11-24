using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Medical
{
    public class MedicalDetail : FullAuditedEntity<long>
    {
        public string BloodGroup { get; set; }

        public string Genotype { get; set; }

        public bool Disability { get; set; }

        public string Allergies { get; set; }
        public string ConfidentialNotes { get; set; }
        public List<ImmunizationHistory> ImmunizationHistories { get; set; }



    }
}
