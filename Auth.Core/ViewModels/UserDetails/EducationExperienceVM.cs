using Auth.Core.Models.UserDetails;
using System;

namespace Auth.Core.ViewModels.Staff
{
    public class EducationExperienceVM
    {
        public string EducationSchoolName { get; set; }
        public string EducationSchoolQualification { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public static implicit operator EducationExperienceVM(EducationExperience model)
        {
            return model == null ? null : new EducationExperienceVM
            {
                EducationSchoolName = model.EducationSchoolName,
                EducationSchoolQualification = model.EducationQualification,
                EndDate = model.EndDate,
                StartDate = model.StartDate

            };
        }
    }
}
