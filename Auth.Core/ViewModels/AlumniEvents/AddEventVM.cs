﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.AlumniEvent
{
    public class AddEventVM
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string Location { get; set; }
        public List<string> EventTags { get; set; }
        public IFormFile file { get; set; }
    }
}
