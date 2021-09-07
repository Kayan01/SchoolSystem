using System;

namespace Auth.Core.ViewModels.AlumniEvent
{
    public class UpdateEventVM
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
