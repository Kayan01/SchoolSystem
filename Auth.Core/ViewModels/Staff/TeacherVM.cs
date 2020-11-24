using Auth.Core.Models.Users;
using Auth.Core.ViewModels.SchoolClass;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using System;
using System.Collections.Generic;
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
        public List<WorkExperienceVM> WorkExperienceVMs { get; set; }
        public List<EducationExperienceVM> EducationExperienceVMs { get; set; }
        public List<DocumentType> DocumentTypes { get; set; }
        public List<IFormFile> Files { get; set; }
    }

    public class UpdateTeacherVM
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public long? ClassId { get; set; }
    }

    public class TeacherDetailVM : StaffDetailVM
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public static implicit operator TeacherDetailVM(TeachingStaff model)
        {
            return model == null ? null : new TeacherDetailVM
            {
                Id = model.Id,
                Email = model.Staff?.User?.Email,
                FirstName = model.Staff?.User?.FirstName,
                LastName = model.Staff?.User?.LastName,
                PhoneNumber = model.Staff?.User?.PhoneNumber,
            };
        }
    }
}
