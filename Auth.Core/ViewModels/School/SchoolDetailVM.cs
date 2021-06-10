using System;
using System.Linq;
using Microsoft.OpenApi.Extensions;
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
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string Icon { get; set; }
        public string Logo { get; set; }

        public long? TotalUsersCount { get; set; }
        public long? StaffCount { get; set; }
        public long? StudentsCount { get; set; }
        public long? TeachersCount { get; set; }

        //public static explicit operator SchoolDetailVM(Models.School model)
        //{
        //    var ct = model.SchoolContactDetails?.FirstOrDefault(x => x.IsPrimaryContact);
        //    var logoId = model.FileUploads?.FirstOrDefault(x => x.Name == DocumentType.Logo.GetDisplayName());
        //    var iconId = model.FileUploads?.FirstOrDefault(x => x.Name == DocumentType.Icon.GetDisplayName());
        //    var staffCount = model.Staffs?.Count();
        //    var studentCount = model.Students?.Count();
        //    var teachersCount = model.TeachingStaffs?.Count();

        //    return model == null ? null : new SchoolDetailVM
        //    {
        //        Id = model.Id,
        //        Name = model.Name,
        //        DateCreated = model.CreationTime,
        //        Status = true, //TODO    
        //        ClientCode = model.ClientCode,
        //        Address = model.Address,
        //        City = model.City,
        //        ContactEmail = ct?.Email,
        //        ContactFirstName = ct?.FirstName,
        //        ContactLastName = ct?.LastName,
        //        ContactPhone = ct?.PhoneNumber,
        //        Country = model.Country,
        //        DomainName = model.DomainName,
        //        State = model.State,
        //        WebsiteAddress = model.WebsiteAddress,
        //        TotalUsersCount = staffCount + studentCount + teachersCount,
        //        StudentsCount = studentCount,
        //        StaffCount = staffCount,
        //        TeachersCount = teachersCount,
        //        Logo = logoId?.Path?.GetBase64StringFromImage(),
        //        Icon = iconId?.Path?.GetBase64StringFromImage()

        //    };
        //}
    }
}
