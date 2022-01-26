using System;
using System.Collections.Generic;
using System.Text;
using Auth.Core.Models;
using Shared.Enums;
using Shared.Utils;

namespace Auth.Core.ViewModels.Student
{
    public class StudentDetailVM
    {
        public long TenantId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MothersMaidenName { get; set; }
        public string Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ParentName => $"{ParentFirstName} {ParentLastName}";
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string ParentFirstName { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string  ParentLastName { get; set; }
        public long? ParentId { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string LocalGovernment { get; set; }
        public string StateOfOrigin { get; set; }
        public string StudentType { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string Level { get; set; }
        public string EntryType { get; set; }        
        public long SectionId { get; set; }
        public string Section { get; set; }
        public string Class => SchoolClass?.FullName;
        public long? ClassId => SchoolClass?.Id;

        public long? SchoolSectionid => SchoolClass?.SchoolSectionId;
        
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Models.SchoolClass SchoolClass { get; set; }
        public string BloodGroup { get; set; }
        public string Genotype { get; set; }
        public string Allergies { get; set; }
        public string ConfidentialNote { get; set; }
        public List<ImmunizationHistoryVM>  ImmunizationHistoryVMs { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Country { get; set; }
        public string HomeAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Image { get; set; }
        public long Id { get; set; }
        public string RegNumber { get; internal set; }
        public bool IsActive { get; set; }
        public bool Disability { get; set; }
    }

    public class ImmunizationHistoryVM
    {
        public DateTime DateImmunized { get; set; }
        public int Age { get; set; }
        public string Vaccine { get; set; }

    }
    public class StudentVM
    {
        public long Id { get; internal set; }
        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public string StudentNumber { get; set; }
        public string Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Section { get; set; }
        public string Class => SchoolClass?.FullName;
        public bool IsActive { get; set; }
        public string Email { get;  set; }
        public string PhoneNumber { get; set; }
        public long? UserId { get; set; }
        public byte[] Image => ImagePath?.GetBase64StringFromImage();

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string ImagePath { get; set; }


        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Models.SchoolClass SchoolClass { get; set; }
        public static implicit operator StudentVM(Models.Student model)
        {
            return model == null ? null : new StudentVM
            {
                Id = model.Id,
                FirstName = model.User.FirstName,
                LastName = model.User.LastName,
                Email = model.User.Email,
                PhoneNumber = model.User.PhoneNumber
            };
        }
    }


    public class StudentVMs
    {
        public long Id { get; internal set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentNumber { get; set; }
        public string Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Section { get; set; }
        public string Class => SchoolClass?.FullName;
        public bool IsActive { get; set; }
        //public byte[] Image => ImagePath?.GetBase64StringFromImage();

        //[System.Text.Json.Serialization.JsonIgnore]
        //[Newtonsoft.Json.JsonIgnore]
        
        public string Image { get; set; }

        public Models.SchoolClass SchoolClass { get; set; }
        public static implicit operator StudentVMs(Models.Student model)
        {
            return model == null ? null : new StudentVMs
            {
                Id = model.Id,
                FirstName = model.User.FirstName,
                LastName = model.User.LastName,
                StudentNumber = model.RegNumber,
                Sex = model.Sex,
                Section = model.Class.SchoolSection.Name,
                IsActive = model.IsActive
            };
        }
    }
}
