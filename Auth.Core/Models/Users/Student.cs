using Auth.Core.Models.Medical;
using Auth.Core.Models.Users;
using Shared.Entities.Auditing;
using Shared.Enums;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Auth.Core.Models
{
    public class Student : Person
    {
        public string MothersMaidenName { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string StateOfOrigin { get; set; }
        public string LocalGovernment { get; set; }
        public string TransportRoute { get; set; }
        public string EntryType { get; set; }
        public DateTime AdmissionDate { get; set; }
        public StudentType StudentType { get; set; }
        public string Level { get; set; }
        public string Country { get; set; }
        public string Town { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public long ParentId { get; set; }
        public long MedicalDetailID { get; set; }
        public long SectionId { get; set; }
        public long? ClassId { get; set; }
        public SchoolClass Class { get; set; }
        public SchoolSection Section { get; set; }
        public Parent Parent { get; set; }
        public MedicalDetail  MedicalDetail { get; set; }
    }
}