﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Auth.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Auth.Core.ViewModels
{
   public class SchoolVM
    {
        public long Id { get; internal set; }
        public string Name { get; set; }
        public string ClientCode { get; set; }
        public long UsersCount { get; set; }
        public string State { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreated { get; set; }

        public static implicit operator SchoolVM(Models.School model)
        {
            return model == null ? null : new SchoolVM
            {
                Id = model.Id,
                Name = model.Name
            };
        }

    }
}
