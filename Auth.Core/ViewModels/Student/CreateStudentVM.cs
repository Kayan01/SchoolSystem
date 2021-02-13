using Microsoft.AspNetCore.Http;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auth.Core.ViewModels.Student
{
    public class CreateStudentVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string MothersMaidenName { get; set; }
        public string Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string StateOfOrigin { get; set; }
        public string LocalGovt { get; set; }
        public string TransportRoute { get; set; }
        public string EntryType { get; set; }
        public DateTime AdmissionDate { get; set; }
        [Required]
        public long SectionId { get; set; }
        [Required]
        public long ClassId { get; set; }
        public StudentType StudentType { get; set; }
        public string Level { get; set; }
        public string ContactPhone { get; set; }
        public string ContactCountry { get; set; }
        public string ContactTown { get; set; }
        public string ContactEmail { get; set; }
        public string ContactAddress { get; set; }
        public string ContactState { get; set; }
        public string BloodGroup { get; set; }
        public string Genotype { get; set; }
        public bool Disability { get; set; }
        public string Allergies { get; set; }
        public string ConfidentialNotes { get; set; }
        public List<ImmunizationVm> ImmunizationVms { get; set; } = new List<ImmunizationVm>();
        [Required]
        public long ParentId { get; set; }
        public bool IsActive { get; set; }

        public List<IFormFile> Files { get; set; }
        public List<DocumentType> DocumentTypes { get; set; }
    }
}
