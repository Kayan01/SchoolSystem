using Auth.Core.Models.Users;
using Auth.Core.ViewModels.SchoolClass;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auth.Core.ViewModels.Staff
{
    public class TeacherVM : StaffVM
    {

        public static implicit operator TeacherVM (TeachingStaff model)
        {
            return model == null ? null : new TeacherVM
            {
                Id = model.Id,
                Email = model.Staff?.User?.Email,
                FirstName = model.Staff?.User?.FirstName,
                LastName = model.Staff?.User?.LastName,
                PhoneNumber = model.Staff?.User?.PhoneNumber,
                StaffNumber = model.Staff?.RegNumber
            };
        }
    }

    public class AddTeacherVM
    {
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
        public List<WorkExperienceVM> WorkExperienceVMs { get; set; } = new List<WorkExperienceVM>();
        public List<EducationExperienceVM> EducationExperienceVMs { get; set; } = new List<EducationExperienceVM>();
        public List<DocumentType> DocumentTypes { get; set; }
        public List<IFormFile> Files { get; set; }
    }

    public class UpdateTeacherVM : AddTeacherVM
    {
    }

    public class TeacherDetailVM : StaffDetailVM
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public static implicit operator TeacherDetailVM(TeachingStaff model)
        {
            return model == null ? null : new TeacherDetailVM
            {
                NextOfKin = model.Staff.NextOfKin,
                ContactDetails = new ContactDetailsVM
                {
                    Town = model.Staff.Town,
                    Country = model.Staff.Country,
                    Address = model.Staff.Address,
                    AltPhoneNumber = model.Staff.AltPhoneNumber,
                    AltEmailAddress = model.Staff.AltEmailAddress,
                    State = model.Staff.State,
                    EmailAddress = model.Staff.User.Email,
                    PhoneNumber = model.Staff.User.PhoneNumber
                },
                StateOfOrigin = model.Staff.StateOfOrigin,
                State = model.Staff.State,
                Address = model.Staff.Address,
                AltEmailAddress = model.Staff.AltEmailAddress,
                AltPhoneNumber = model.Staff.AltPhoneNumber,
                BloodGroup = model.Staff.BloodGroup,
                Country = model.Staff.Country,
                Id = model.Id,
                DateOfBirth = model.Staff.DateOfBirth,
                RegNumber = model.Staff.RegNumber,
                EmploymentDetails = new EmploymentDetailsVM
                {
                    DepartmentId = model.Staff.DepartmentId,
                    EmploymentDate = model.Staff.EmploymentDate,
                    EmploymentStatus = model.Staff.EmploymentStatus,
                    HighestQualification = model.Staff.HighestQualification,
                    JobTitle = model.Staff.JobTitle,
                    PayGrade = model.Staff.PayGrade,
                    ResumptionDate = model.Staff.ResumptionDate,
                    StaffType = model.Staff.StaffType
                },
                FirstName = model.Staff.User.FirstName,
                IsActive = model.Staff.IsActive,
                LastName = model.Staff.User.LastName,
                LocalGovernment = model.Staff.LocalGovernment,
                MaritalStatus = model.Staff.MaritalStatus,
                Nationality = model.Staff.Nationality,
                OtherNames = model.Staff.User.MiddleName,
                Religion = model.Staff.Religion,
                Sex = model.Staff.Sex,
                Town = model.Staff.Town,
                WorkExperienceVMs = model.Staff.WorkExperiences.Select(x => (WorkExperienceVM)x).ToList(),
                EducationExperienceVMs = model.Staff.EducationExperiences.Select(x => (EducationExperienceVM)x).ToList()

            };
        }
    }
}
