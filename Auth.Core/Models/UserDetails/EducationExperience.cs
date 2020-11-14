using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.UserDetails
{
    public class EducationExperience : FullAuditedEntity<long>
    {
        public string EducationSchoolName { get; set; }
        public string EducationQualification { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
