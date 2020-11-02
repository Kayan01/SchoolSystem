using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
                Name = model.Name,
                DateCreated = model.CreationTime,
                State = model.State,
                UsersCount = model.Staffs.Count() + model.TeachingStaffs.Count() + model.Students.Count(),
                ClientCode = model.ClientCode

            };
        }

        public static implicit operator SchoolVM(UpdateSchoolVM v)
        {
            throw new NotImplementedException();
        }
    }
}
