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
    public class UpdateSchoolGroupVM
    {
        [Required]
        
        public string Name { get; set; }
        [Required]
        
        public string WebsiteAddress { get; set; }
        
        public string PrimaryColor { get; set; }
        
        public string SecondaryColor { get; set; }
        [Required]
        
        public string ContactFirstName { get; set; }
        [Required]
        
        public string ContactLastName { get; set; }
        [Required]
        
        public string ContactPhoneNo { get; set; }
        [Required]
        
        public string ContactEmail { get; set; }
        public bool IsActive { get; set; } = true;
        public List<DocumentType> DocumentTypes { get; set; }
        public List<IFormFile> Files { get; set; }

    }
}
