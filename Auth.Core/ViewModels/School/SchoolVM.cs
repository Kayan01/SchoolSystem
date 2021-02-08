using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Auth.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Extensions;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Shared.Enums;

namespace Auth.Core.ViewModels
{
    public class SchoolVM
    {
        public long Id { get; internal set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string ClientCode { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string Logo { get; set; }
        public long? UsersCount { get; set; }

        public static implicit operator SchoolVM(Models.School model)
        {
            //var cont = model.SchoolContactDetails?.FirstOrDefault(x => x.IsPrimaryContact);
            var staffCount = model.Staffs?.Count();
            var studentCount = model.Students?.Count();
            var teachersCount = model.TeachingStaffs?.Count();
            var fileId = model.FileUploads?.FirstOrDefault(x => x.Name == DocumentType.Logo.GetDisplayName())?.Path;

            return model == null ? null : new SchoolVM
            {
                Id = model.Id,
                Name = model.Name,
                DateCreated = model.CreationTime,
                Status = true, //TODO    
                State = model.State,
                UsersCount = studentCount + teachersCount + staffCount,
                ClientCode = model.ClientCode,
               // Logo = fileId.GetBase64StringFromImage(),
            };
        }

        public static implicit operator SchoolVM(UpdateSchoolVM v)
        {
            throw new NotImplementedException();
        }
    }
}
