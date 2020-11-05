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
using Shared.Utils;

namespace Auth.Core.ViewModels
{
    public class SchoolDetailVM
    {
        public long Id { get; internal set; }
        public string Name { get; set; }
        public string ClientCode { get; set; }
        public string DomainName { get; set; }
        public string WebsiteAddress { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreated { get; set; }
        public byte[] Icon { get; set; }
        public byte[] Logo { get; set; }

        public long? TotalUsersCount { get; set; }
        public long? StaffCount { get; set; }
        public long? StudentsCount { get; set; }
        public long? TeachersCount { get; set; }

        public static explicit operator SchoolDetailVM(Models.School model)
        {
            var ct = model.SchoolContactDetails?.FirstOrDefault(x => x.IsPrimaryContact);
            var logoId = model.FileUploads?.FirstOrDefault(x => x.Name == DocumentType.Logo.GetDisplayName());
            var iconId = model.FileUploads?.FirstOrDefault(x => x.Name == DocumentType.Icon.GetDisplayName());
            var staffCount = model.Staffs?.Count();
            var studentCount = model.Students?.Count();
            var teachersCount = model.TeachingStaffs?.Count();

            return model == null ? null : new SchoolDetailVM
            {
                Id = model.Id,
                Name = model.Name,
                DateCreated = model.CreationTime,
                Status = true, //TODO    
                ClientCode = model.ClientCode,
                Address = model.Address,
                City = model.City,
                ContactEmail = ct?.Email,
                ContactFirstName = ct?.FirstName,
                ContactLastName = ct?.LastName,
                ContactPhone = ct?.PhoneNumber,
                Country = model.Country,
                DomainName = model.DomainName,
                State = model.State,
                WebsiteAddress = model.WebsiteAddress,
                TotalUsersCount = staffCount + studentCount + teachersCount,
                StudentsCount = studentCount,
                StaffCount = staffCount,
                TeachersCount = teachersCount,
                Logo = logoId?.Path?.GetBase64StringFromImage(),
                Icon = iconId?.Path?.GetBase64StringFromImage()

            };
        }
    }
    public class SchoolVM
    {
        public long Id { get; internal set; }
        public string Name { get; set; }
        public string ClientCode { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreated { get; set; }
        public byte[] Logo { get; set; }
        public long? UsersCount { get; set; }

        public static implicit operator SchoolVM(Models.School model)
        {
            var cont = model.SchoolContactDetails?.FirstOrDefault(x => x.IsPrimaryContact);
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
                UsersCount = studentCount + teachersCount + staffCount,
                ClientCode = model.ClientCode,
                Logo = fileId.GetBase64StringFromImage(),
            };
        }

        public static implicit operator SchoolVM(UpdateSchoolVM v)
        {
            throw new NotImplementedException();
        }
    }
}
