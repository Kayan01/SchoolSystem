using Auth.Core.Models.Setup;
using Auth.Core.Models.UserDetails;
using Auth.Core.Models.Users;
using Shared.Enums;
using System;
using System.Collections.Generic;

namespace Auth.Core.Models
{
    public class Staff : Person
    {
        public string MaritalStatus { get; set; }
        public string BloodGroup { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string StateOfOrigin { get; set; }
        public string LocalGovernment { get; set; }
        public string EmploymentStatus { get; set; }
        public string HighestQualification { get; set; }
        public string JobTitle { get; set; }
        public string PayGrade { get; set; }
        public DateTime EmploymentDate { get; set; }
        public DateTime ResumptionDate { get; set; }
        public bool IsActive { get; set; }
        public StaffType StaffType { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Town { get; set; }
        public string AltPhoneNumber { get; set; }
        public string AltEmailAddress { get; set; }

        public int DepartmentId { get; set; }
        public Department  Department { get; set; }
        public int NextOfKinId { get; set; }
        public NextOfKin NextOfKin { get; set; }
        public List<WorkExperience>  WorkExperiences { get; set; }
        public List<EducationExperience> EducationExperiences { get; set; }
   

    }
}