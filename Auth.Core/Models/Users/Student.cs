using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Auth.Core.Models
{
    public class Student : Person
    {
        public long? ClassId { get; set; }
        public SchoolClass Class { get; set; }
    }
}