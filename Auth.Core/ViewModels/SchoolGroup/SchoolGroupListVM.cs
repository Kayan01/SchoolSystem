using System;
using System.Linq;
using Auth.Core.ViewModels.SchoolGroup;
using Microsoft.OpenApi.Extensions;
using Shared.Enums;
using Shared.Utils;

namespace Auth.Core.ViewModels
{
    public class SchoolGroupListVM
    {
        public long Id { get; internal set; }
        public string Name { get; set; }
        public string WebsiteAddress { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public bool IsActive { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public DateTime DateCreated { get; set; }
        public static implicit operator SchoolGroupListVM(Models.SchoolGroup model)
        {
            var ct = model.SchoolContactDetails?.FirstOrDefault(x => x.IsPrimaryContact);
            //var logoId = model.FileUploads?.FirstOrDefault(x => x.Name == DocumentType.Logo.GetDisplayName());
            //var iconId = model.FileUploads?.FirstOrDefault(x => x.Name == DocumentType.Icon.GetDisplayName());
            //var staffCount = model.Staffs?.Count();
            //var studentCount = model.Students?.Count();
            //var teachersCount = model.TeachingStaffs?.Count();

            return model == null ? null : new SchoolGroupListVM
            {
                Id = model.Id,
                Name = model.Name,
                DateCreated = model.CreationTime,
                IsActive = model.IsActive,
                ContactEmail = ct?.Email,
                ContactFirstName = ct?.FirstName,
                ContactLastName = ct?.LastName,
                ContactPhone = ct?.PhoneNumber,
                WebsiteAddress = model.WebsiteAddress,
                PrimaryColor = model.PrimaryColor,
                SecondaryColor = model.SecondaryColor
            };
        }
    }
}
