using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auth.Core.ViewModels.Staff
{
    public class StaffVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Id { get;  set; }
        public long UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string StaffType { get; set; }

        public static implicit operator StaffVM(Models.Staff model)
        {
            return model == null ? null : new StaffVM
            {
                Id = model.Id,
            };
        }
    }


    public class StaffDetailVM
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Sex { get; set; }
        public string MaritalStatus { get; set; }
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


        public static implicit operator StaffDetailVM(Models.Staff model)
        {
            return model == null ? null : new StaffDetailVM
            {
                NextOfKin = model.NextOfKin,
                ContactDetails = new ContactDetailsVM
                {
                    Town = model.Town,
                    Country = model.Country,
                    Address = model.Address,
                    AltPhoneNumber = model.AltPhoneNumber,
                    AltEmailAddress = model.AltEmailAddress,
                    State = model.State,
                    EmailAddress = model.User.Email,
                    PhoneNumber = model.User.PhoneNumber
                },
                StateOfOrigin = model.StateOfOrigin,
                State = model.State,
                Address = model.Address,
                AltEmailAddress = model.AltEmailAddress,
                AltPhoneNumber = model.AltPhoneNumber,
                BloodGroup = model.BloodGroup,
                Country = model.Country,
                Id = model.Id,
                DateOfBirth = model.DateOfBirth,
                EmploymentDetails = new EmploymentDetailsVM
                {
                    DepartmentId = model.DepartmentId,
                    EmploymentDate = model.EmploymentDate,
                    EmploymentStatus = model.EmploymentStatus,
                    HighestQualification = model.HighestQualification,
                    JobTitle = model.JobTitle,
                    PayGrade = model.PayGrade,
                    ResumptionDate = model.ResumptionDate,
                    StaffType = model.StaffType
                },
                FirstName = model.User.FirstName,
                IsActive = model.IsActive,
                LastName = model.User.LastName,
                LocalGovernment = model.LocalGovernment,
                MaritalStatus = model.MaritalStatus,
                Nationality = model.Nationality,
                OtherNames = model.User.MiddleName,
                Religion = model.Religion,
                Sex = model.Sex,
                Town = model.Town,
                WorkExperienceVMs = model.WorkExperiences.Select(x => (WorkExperienceVM)x).ToList(),
                EducationExperienceVMs = model.EducationExperiences.Select(x => (EducationExperienceVM)x).ToList()

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
        public string StateOfOrigin { get; set; }
        public string LocalGovernment { get; set; }
        public bool IsActive { get; set; }

        public NextOfKinVM NextOfKin { get; set; }
        public ContactDetailsVM ContactDetails { get; set; }

        public EmploymentDetailsVM EmploymentDetails { get; set; }
        public List<WorkExperienceVM> WorkExperienceVMs { get; set; } = new List<WorkExperienceVM>();
        public List<EducationExperienceVM> EducationExperienceVMs { get; set; } = new List<EducationExperienceVM>();
        public List<DocumentType> DocumentTypes { get; set; }
        public List<IFormFile> Files { get; set; }

    }
}
