using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Setup
{
    public class SchoolPropertyVM
    {
        public string Prefix { get; set; }
        public string Seperator { get; set; }
        public long EnrollmentAmount { get; set; }
        public int NumberOfTerms { get; set; }
        public ClassDaysType ClassDays { get; set; }
    }


}
