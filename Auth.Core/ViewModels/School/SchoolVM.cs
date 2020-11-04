using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Auth.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Extensions;
using Shared.Enums;

namespace Auth.Core.ViewModels
{
    public class SchoolVM
    {
        public long Id { get; internal set; }
        public string Name { get; set; }
        public string ClientCode { get; set; }
        public string DomainName { get; set; }
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
        public Guid? IconId { get; set; }
        public Guid? LogoId { get; set; }
        public long? UsersCount { get; set; }
        public long? StaffCount { get; set; }
        public long? StudentsCount { get; set; }
        public long? TeachersCount { get; set; }

        public static implicit operator SchoolVM(Models.School model)
        {
            var cont = model.SchoolContactDetails?.FirstOrDefault(x => x.IsPrimaryContact);
            var staffCount = model.Staffs?.Count();
            var studentCount = model.Students?.Count();
            var teachersCount = model.TeachingStaffs?.Count();


            return model == null ? null : new SchoolVM
            {
                Id = model.Id,
                Name = model.Name,
                DateCreated = model.CreationTime,
                DomainName = model.DomainName,
                State = model.State,
                Country = model.Country,
                Address = model.Address,
                City = model.City,
                Status = true, //TODO      
                UsersCount = staffCount + studentCount + teachersCount,
                StudentsCount = studentCount,
                StaffCount = staffCount,
                TeachersCount = teachersCount,
                ClientCode = model.ClientCode,
                IconId = model.FileUploads?.FirstOrDefault(x => x.Name == DocumentType.Icon.GetDisplayName())?.Id,
                LogoId = model.FileUploads?.FirstOrDefault(x => x.Name == DocumentType.Logo.GetDisplayName())?.Id,
                ContactEmail =cont?.Email,
                 ContactFirstName = cont?.FirstName,
                ContactLastName = cont?.LastName,
                 ContactPhone = cont?.PhoneNumber,

            };
        }

        public static implicit operator SchoolVM(UpdateSchoolVM v)
        {
            throw new NotImplementedException();
        }
    }
}
