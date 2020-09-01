using Auth.Core.Enumerations;
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
        public int UserId { get;  set; }
        public StaffType StaffType { get; set; }
    }
}
