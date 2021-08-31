using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auth.Core.ViewModels.SchoolGroup
{
    public class GetSchoolGroupAnalyticsVM
    {
        public int NoOfSchools { get; set; }
        public int NoOfStudents {
            get
            {
                return StudentCounts.Sum();
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public IEnumerable<int> StudentCounts { get; set; } = new List<int>();
    }
}
