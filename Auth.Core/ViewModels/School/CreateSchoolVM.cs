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
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string DomainName { get; set; }
        [Required]
        public string WebsiteAddress { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string ContactFirstName { get; set; }
        [Required]
        public string ContactLastName { get; set; }
        [Required]
        public string ContactPhoneNo { get; set; }
        [Required]
        public string ContactEmail { get; set; }

        public List<DocumentType> DocumentTypes { get; set; }
        public List<IFormFile> Files { get; set; }

    }
}
