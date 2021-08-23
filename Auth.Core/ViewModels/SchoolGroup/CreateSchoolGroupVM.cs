using ExcelManager;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Auth.Core.ViewModels.SchoolGroup
{
    public class CreateSchoolGroupVM
    {
        [Required]
        [ExcelReaderCell]
        public string Name { get; set; }
        [Required]
        [ExcelReaderCell]
        public string WebsiteAddress { get; set; }
        [ExcelReaderCell]
        public string PrimaryColor { get; set; }
        [ExcelReaderCell]
        public string SecondaryColor { get; set; }
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
        public bool IsActive { get; set; } = true;
        public List<DocumentType> DocumentTypes { get; set; }
        public List<IFormFile> Files { get; set; }

    }
}
