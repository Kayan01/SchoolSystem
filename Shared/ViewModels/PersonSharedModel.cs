﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class PersonSharedModel
    {
        public long UserId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RegNumber { get; set; }
    }
}
