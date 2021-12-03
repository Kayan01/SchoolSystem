using ExcelManager;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Auth.Core.ViewModels.School
{
    public class CreateSchoolVM
    {
        [Required]
        [ExcelReaderCell]
        public string Name { get; set; }
        [Required]
        [ExcelReaderCell]
        public string DomainName { get; set; }
        [Required]
        [ExcelReaderCell]
        public string WebsiteAddress { get; set; }
        [Required]
        [ExcelReaderCell]
        public string Address { get; set; }
        [Required]
        [ExcelReaderCell]
        public string City { get; set; }
        [Required]
        [ExcelReaderCell]
        public string State { get; set; }
        [Required]
        [ExcelReaderCell]
        public string Country { get; set; }
        [Required]
        [ExcelReaderCell]
        public string ContactFirstName { get; set; }
        [Required]
        [ExcelReaderCell]
        public string ContactLastName { get; set; }
        [Required]
        [ExcelReaderCell]
        public string ContactPhoneNo { get; set; }
        [Required]
        [ExcelReaderCell]
        public string ContactEmail { get; set; }
        [Required]
        [ExcelReaderCell]
        public string Username { get; set; }
        [Required]
        [ExcelReaderCell]
        public string ContactEmailPassword { get; set; }
        public bool IsActive { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public long? GroupId { get; set; }

        public List<DocumentType> DocumentTypes { get; set; }
        public List<IFormFile> Files { get; set; }



    }
}
