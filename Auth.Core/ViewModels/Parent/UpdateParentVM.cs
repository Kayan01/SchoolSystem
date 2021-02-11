using Microsoft.AspNetCore.Http;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.ViewModels.Parent
{
    public class UpdateParentVM
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public string Sex { get; set; }
        public string Occupation { get; set; }

        public string ModeOfIdentification { get; set; }
        public string IdentificationNumber { get; set; }
        public bool Status { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public string SecondaryEmailAddress { get; set; }
        public string HomeAddress { get; set; }
        public string OfficeAddress { get; set; }
        public IFormFile File { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}
