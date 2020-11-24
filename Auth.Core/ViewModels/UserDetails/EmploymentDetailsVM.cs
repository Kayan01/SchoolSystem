using Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Auth.Core.ViewModels.Staff
{
    public class EmploymentDetailsVM
    {
        public int DepartmentId { get; set; }
        [Required]
        public StaffType StaffType { get; set; }
        public string EmploymentStatus { get; set; }
        public string HighestQualification { get; set; }
        public string JobTitle { get; set; }
        public string PayGrade { get; set; }
        public DateTime EmploymentDate { get; set; }
        public DateTime ResumptionDate { get; set; }
    }
}
