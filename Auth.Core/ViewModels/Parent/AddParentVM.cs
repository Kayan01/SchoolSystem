using Microsoft.AspNetCore.Http;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ExcelManager;

namespace Auth.Core.ViewModels.Parent
{
    public class AddParentVM
    {
        [ExcelReaderCell]

        public string Title { get; set; }
        [ExcelReaderCell]

        public string FirstName { get; set; }
        [ExcelReaderCell]

        public string  LastName { get; set; }
        [ExcelReaderCell]

        public string OtherName { get; set; }
        [ExcelReaderCell]

        public string Sex { get; set; }
        [ExcelReaderCell]

        public string Occupation { get; set; }
        [ExcelReaderCell]


        public string ModeOfIdentification { get; set; }
        [ExcelReaderCell]

        public string IdentificationNumber { get; set; }
        [ExcelReaderCell]

        public bool Status { get; set; }
        [ExcelReaderCell]

        public string PhoneNumber { get; set; }
        [ExcelReaderCell]

        public string EmailAddress { get; set; }
        [ExcelReaderCell]

        public string SecondaryPhoneNumber { get; set; }
        [ExcelReaderCell]

        public string SecondaryEmailAddress { get; set; }
        [ExcelReaderCell]

        public string HomeAddress { get; set; }
        [ExcelReaderCell]

        public string OfficeAddress { get; set; }
        public IFormFile File { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}
