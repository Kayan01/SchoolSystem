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
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MothersMaidenName { get; set; }
        public string Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ParentName { get; set; }
        public long ParentId { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string LocalGovernment { get; set; }
        public string StateOfOrigin { get; set; }
        public string StudentType { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string Section { get; set; }
        public string Class { get; set; }
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
        public string Class { get; set; }
        public bool IsActive { get; set; }
        public string Email { get;  set; }
        public string PhoneNumber { get; set; }
        public byte[] Image => ImagePath?.GetBase64StringFromImage();

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string ImagePath { get; set; }
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
}
