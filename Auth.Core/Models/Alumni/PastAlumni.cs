using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Alumni
{
    public class PastAlumni : FullAuditedEntity<long>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string RegNumber { get; set; }
        public long SchoolId { get; set; }
        public string SessionName { get; set; }
        public string EmailAddress { get; set; }
        public string TermName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MothersMaidenName { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string StateOfOrigin { get; set; }
        public string LocalGovernment { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string AlumniReason { get; set; }
        public DateTime YearOfCompletion { get; set; }

    }
}
