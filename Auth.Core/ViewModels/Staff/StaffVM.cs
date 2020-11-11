using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Staff
{
    public class StaffVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Id { get;  set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string StaffType { get; set; }

        public static implicit operator StaffVM(Models.Staff model)
        {
            return model == null ? null : new StaffVM
            {
                Id = model.Id,
            };
        }
    }

    public class AddStaffVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Sex { get; set; }
        public string  MaritalStatus { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BloodGroup { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string Country { get; set; }
        public string StateOfOrigin { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Town { get; set; }
        public string LocalGovernment { get; set; }
        public string AltPhoneNumber { get; set; }
        public string AltEmailAddress { get; set; }
        public bool IsActive { get; set; }

        public NextOfKinVM NextOfKin { get; set; }
        public ContactDetailsVM ContactDetails { get; set; }

        public EmploymentDetailsVM EmploymentDetails { get; set; }
        public List<WorkExperienceVM> WorkExperienceVMs { get; set; }
        public List<EducationExperienceVM> EducationExperienceVMs { get; set; }
        public List<DocumentType> DocumentTypes { get; set; }
        public List<IFormFile> Files { get; set; }

    }
}
