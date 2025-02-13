﻿using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class StudentSharedModel : PersonSharedModel
    {
        public long TenantId { get; set; }
        public long Id { get; set; }
        public string ParentName { get; set; }
        public string ParentEmail { get; set; }
        public long? ClassId { get; set; }
        public long ParentId { get; set; }
        public string Sex { get; set; }
        public DateTime DoB { get; set; }
        public StudentStatusInSchool StudentStatusInSchool { get; set; }
    }
}
