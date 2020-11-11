using System;

namespace Auth.Core.ViewModels.Staff
{
    public class WorkExperienceVM
    {
        public string WorkRole { get; set; }
        public string WorkCompanyName { get; set; }

        public  DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
