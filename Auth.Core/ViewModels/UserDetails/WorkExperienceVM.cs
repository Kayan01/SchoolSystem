using Auth.Core.Models.UserDetails;
using System;

namespace Auth.Core.ViewModels.Staff
{
    public class WorkExperienceVM
    {
        public string WorkRole { get; set; }
        public string WorkCompanyName { get; set; }

        public  DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public static implicit operator WorkExperienceVM(WorkExperience model)
        {
            return model == null ? null : new WorkExperienceVM
            {
                EndTime = model.EndTime,
                StartTime = model.StartTime,
                WorkCompanyName = model.WorkCompanyName,
                WorkRole = model.WorkRole

            };
        }

    }
}
