using Auth.Core.Enumerations;
using Auth.Core.Models.Users;
using NPOI.SS.Formula.Functions;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models
{
    public class Staff : Person
    {
        public StaffType StaffType { get; set; }
        public TeachingStaff TeachingStaff { get; set; }
    }
}