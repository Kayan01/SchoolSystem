using System;

namespace Auth.Core.ViewModels.Staff
{
    public class EducationExperienceVM
    {
        public string EducationSchoolName { get; set; }
        public string EducationSchoolQualification { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
